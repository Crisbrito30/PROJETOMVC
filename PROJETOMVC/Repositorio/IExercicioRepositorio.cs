using PROJETOMVC.Models;
using PROJETOMVC.Models.Enums;


namespace PROJETOMVC.Repositorio
{
    public interface IExercicioRepositorio
    {
        //CRUD Básico
        Task<Exercicio?> BuscarPorIdAsync(int id);
        Task<IEnumerable<Exercicio>> BuscarTodosAsync();
        Task<Exercicio> AdicionarAsync(Exercicio exercicio);
        Task<Exercicio> AtualizarAsync(Exercicio exercicio);
        Task<bool> DeletarAsync(int id);

        //Buscar específico
        Task<IEnumerable<Exercicio>> BuscarPorNomeAsync(string nome);
        Task<IEnumerable<Exercicio>> BuscarPorGrupoMuscularAsync(GrupoMuscular grupoMuscular);
        Task<IEnumerable<Exercicio>> BuscarAtivosAsync();
    }
}
