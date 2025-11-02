using AcademiaApp.Data;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using PROJETOMVC.Models;
using PROJETOMVC.Models.Enums;

namespace PROJETOMVC.Repositorio
{
    public class ExercicioRepositorio : IExercicioRepositorio
    {
        private readonly AcademiaContext _context;

        public ExercicioRepositorio(AcademiaContext context)
        {
            _context = context;
        }

        //Crud Exercício
        public async Task<Exercicio> AdicionarAsync(Exercicio exercicio)
        {
            await _context.Exercicios.AddAsync(exercicio);
            await _context.SaveChangesAsync();
            return exercicio;
        }

        public async Task<Exercicio> AtualizarAsync(Exercicio exercicio)
        {
            var exercicioBanco = await BuscarPorIdAsync(exercicio.Id);

            if (exercicioBanco == null)
                throw new Exception("Exercício não encontrado!");

            // Atualiza os campos
            exercicioBanco.Nome = exercicio.Nome;
            exercicioBanco.GrupoMuscular = exercicio.GrupoMuscular;
            exercicioBanco.Descricao = exercicio.Descricao;
            exercicioBanco.VideoUrl = exercicio.VideoUrl;
            exercicioBanco.Ativo = exercicio.Ativo;

            _context.Exercicios.Update(exercicioBanco);
            await _context.SaveChangesAsync();
            return exercicioBanco;
        }
        public async Task<bool> DeletarAsync(int id)
        {
            var exercicioBanco = await BuscarPorIdAsync(id);

            if (exercicioBanco == null)
            {
                throw new Exception("Exercício não encontrado!");
            }

            _context.Exercicios.Remove(exercicioBanco);
            _context.SaveChanges();
            return true;
        }

        //BUSCAS ESPEFICIAS
        public async Task<Exercicio?> BuscarPorIdAsync(int id)
        {
            return await _context.Exercicios
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Exercicio>> BuscarAtivosAsync()
        {
            return await _context.Exercicios
                .Where(e => e.Ativo)
                .OrderBy(e => e.GrupoMuscular)
                .ThenBy(e => e.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercicio>> BuscarPorGrupoMuscularAsync(GrupoMuscular grupoMuscular)
        {
            return await _context.Exercicios
                .Where(e => e.GrupoMuscular == grupoMuscular)
                .OrderBy(e => e.Nome)
                .ToListAsync();

        }
                
        public async Task<IEnumerable<Exercicio>> BuscarPorNomeAsync(string nome)
        {
            return await _context.Exercicios
                 .Where(e => e.Nome.Contains(nome))
                 .OrderBy(e => e.Nome)
                 .AsAsyncEnumerable()
                 .ToListAsync();

        }

        public async Task<IEnumerable<Exercicio>> BuscarTodosAsync()
        {

            return await _context.Exercicios    
                .OrderBy(e => e.GrupoMuscular)  
                .ThenBy(e => e.Nome)    
                .ToListAsync();


        }

        
    }
}

