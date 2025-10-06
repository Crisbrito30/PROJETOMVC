using PROJETOMVC.Models;

namespace PROJETOMVC.Repositorio
{
    public interface IUsuarioRepositorio
    {
        Task<UsuarioModel> AdicionarAsync(UsuarioModel usuario);
        Task<List<UsuarioModel>> BuscarTodosAsync();
        Task<UsuarioModel> ListarPorIdAsync(int id);
        Task<UsuarioModel> AtualizarAsync(UsuarioModel usuario);
        Task<bool> DeletarAsync(int id);

        // ✅ ADICIONE ESTE MÉTODO:
        Task<UsuarioModel> BuscarPorLoginAsync(string login);
    }
}
