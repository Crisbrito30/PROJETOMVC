using PROJETOMVC.Models;

namespace PROJETOMVC.Repositorio
{
    public interface IContatoRepositorio
    {
        Task<ContatoModel> ListarPorIdAsync(int id);
        Task<List<ContatoModel>> BuscarTodosAsync();
        Task<ContatoModel> AdicionarAsync(ContatoModel contato);
        Task<ContatoModel> AtualizarAsync(ContatoModel contato);
        Task<bool> DeletarAsync(int id);
    }
}

