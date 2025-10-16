using System.Collections.Generic;
using System.Threading.Tasks;
using AcademiaApp.Models;

namespace PROJETOMVC.Repositorio
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario> ListarPorIdAsync(int id);
        Task<Usuario?> BuscarPorLoginAsync(string emailOuLogin);
        Task<Usuario?> BuscarPorIdAsync(int id);
        Task<IEnumerable<Usuario>> BuscarTodosAsync();
        Task<Usuario> AdicionarAsync(Usuario usuario);
        Task<Usuario> AtualizarAsync(Usuario usuario);
        Task<bool> DeletarAsync(int id);
    }
}
