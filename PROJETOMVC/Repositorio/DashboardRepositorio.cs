
using Microsoft.EntityFrameworkCore;
using AcademiaApp.Data;
using PROJETOMVC.DTO;

namespace PROJETOMVC.Repositorio
{
    public class DashboardRepositorio : IDashboardRepositorio
    {
        private readonly AcademiaContext _context;

        public DashboardRepositorio(AcademiaContext context) => _context = context;

        public async Task<DashboardMetrics> ObterResumoAsync(DateTime? inicio = null, DateTime? fim = null)
        {
            // Resumos simples
            var totalUsuarios = await _context.Usuarios.CountAsync();
            var totalTreinos = await _context.Treinos.CountAsync();
            var totalExercicios = await _context.Exercicios.CountAsync();

            // Séries
            var usuariosPorMes = await UsuariosPorMesAsync(12);
            var treinosPorDia = await TreinosPorDiaAsync(30);
            var maisUsados = await ExerciciosMaisUsadosAsync(10);
            var adesao = await AdesaoTreinosUltimos7DiasAsync();

            return new DashboardMetrics
            {
                TotalUsuarios = totalUsuarios,
                TotalTreinos = totalTreinos,
                TotalExercicios = totalExercicios,
                UsuariosPorMes = usuariosPorMes,
                TreinosPorDia = treinosPorDia,
                ExerciciosMaisUsados = maisUsados,
                AdesaoTreinosUltimos7Dias = adesao
            };
        }

        /// <summary>
        /// Usuários cadastrados por mês (usa UsuarioModel.DataCadastro)
        /// </summary>
        public async Task<IEnumerable<SerieTemporal>> UsuariosPorMesAsync(int meses = 12)
        {
            var limite = DateTime.Now.AddMonths(-meses + 1); // local time
            return await _context.Usuarios
                .Where(u => u.DataCadastro >= limite)
                .GroupBy(u => new { u.DataCadastro.Year, u.DataCadastro.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new SerieTemporal
                {
                    Label = $"{g.Key.Year}-{g.Key.Month:00}",
                    Valor = g.Count()
                })
                .ToListAsync();
        }

        /// <summary>
        /// Treinos criados por dia (usa Treino.DataCriacao).
        /// Se você preferir contar execuções por dia, trocaremos para ExecucaoTreino.DataExecucao.
        /// </summary>
        public async Task<IEnumerable<SerieTemporal>> TreinosPorDiaAsync(int dias = 30)
        {
            var limite = DateTime.Now.Date.AddDays(-dias + 1); // local date
            return await _context.Treinos
                .Where(t => t.DataCriacao.Date >= limite)
                .GroupBy(t => t.DataCriacao.Date)
                .OrderBy(g => g.Key)
                .Select(g => new SerieTemporal
                {
                    Label = g.Key.ToString("yyyy-MM-dd"),
                    Valor = g.Count()
                })
                .ToListAsync();
        }

        /// <summary>
        /// Exercícios mais usados (conta quantas vezes aparecem em TreinoExercicio).
        /// </summary>
        public async Task<IEnumerable<ItemQuantidade>> ExerciciosMaisUsadosAsync(int top = 10)
        {
            // TODO: confirme o nome da entidade ponte TreinoExercicio e suas chaves: ExercicioId, TreinoId
            var query = _context.TreinoExercicios
                .GroupBy(te => te.ExercicioId)
                .Select(g => new { ExercicioId = g.Key, Quantidade = g.Count() })
                .OrderByDescending(x => x.Quantidade)
                .Take(top)
                .Join(_context.Exercicios,
                    x => x.ExercicioId,
                    e => e.Id,
                    (x, e) => new ItemQuantidade { Nome = e.Nome, Quantidade = x.Quantidade });

            return await query.ToListAsync();
        }

        /// <summary>
        /// Adesão nos últimos 7 dias: conta treinos por dia (DataCriacao).
        /// Se quiser "execuções por dia", trocamos para ExecucaoTreino.
        /// </summary>
        public async Task<IEnumerable<ItemQuantidade>> AdesaoTreinosUltimos7DiasAsync()
        {
            var limite = DateTime.Now.Date.AddDays(-6);
            var porDia = await _context.Treinos
                .Where(t => t.DataCriacao.Date >= limite)
                .GroupBy(t => t.DataCriacao.Date)
                .Select(g => new ItemQuantidade
                {
                    Nome = g.Key.ToString("yyyy-MM-dd"),
                    Quantidade = g.Count()
                })
                .ToListAsync();

            // Normaliza (dias sem dados = zero)
            var lista = new List<ItemQuantidade>();
            for (int i = 0; i < 7; i++)
            {
                var diaStr = limite.AddDays(i).ToString("yyyy-MM-dd");
                lista.Add(porDia.FirstOrDefault(x => x.Nome == diaStr)
                          ?? new ItemQuantidade { Nome = diaStr, Quantidade = 0 });
            }
            return lista;
        }
    }
}
