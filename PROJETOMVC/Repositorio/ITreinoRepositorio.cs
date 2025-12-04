using System.Collections.Generic;
using System.Threading.Tasks;
using AcademiaApp.Models;
using PROJETOMVC.Models.Enums;
using PROJETOMVC.Models;

namespace PROJETOMVC.Repositorio
{
    public interface ITreinoRepositorio
    {
        // CRUD básico
        Task<Treino?> BuscarPorIdAsync(int id);
        Task<IEnumerable<Treino>> BuscarTodosAsync();
        Task<Treino> AdicionarAsync(Treino treino);
        Task<Treino> AtualizarAsync(Treino treino);
        Task<bool> DeletarAsync(int id);

        // Buscas específicas
        Task<IEnumerable<Treino>> BuscarPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<Treino>> BuscarPorTreinadorAsync(int treinadorId);
        Task<IEnumerable<Treino>> BuscarAtivosAsync();
        Task<IEnumerable<Treino>> BuscarPorNivelAsync(NivelDificuldade nivel);
        Task<IEnumerable<Treino>> BuscarPorTipoAsync(TipoTreino tipo);

        // Busca com relacionamentos
        Task<Treino?> BuscarComExerciciosAsync(int id);
        Task<Treino?> BuscarComExecucoesAsync(int id);
        
        // Adicionar/Remover exercício de um treino
        Task<TreinoExercicio> AdicionarTreinoExercicioAsync(TreinoExercicio treinoExercicio);
    }
}