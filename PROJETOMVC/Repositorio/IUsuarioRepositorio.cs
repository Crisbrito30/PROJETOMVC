using System.Collections.Generic;
using System.Threading.Tasks;
using AcademiaApp.Models;

namespace PROJETOMVC.Repositorio
{
    public interface IUsuarioRepositorio
    {
        Task<UsuarioModel> ListarPorIdAsync(int id);
        Task<UsuarioModel?> BuscarPorLoginAsync(string emailOuLogin);
        Task<UsuarioModel?> BuscarPorIdAsync(int id);
        Task<IEnumerable<UsuarioModel>> BuscarTodosAsync();
        Task<UsuarioModel> AdicionarAsync(UsuarioModel usuario);
        Task<UsuarioModel> AtualizarAsync(UsuarioModel usuario);
        Task<bool> DeletarAsync(int id);

        //Paginação e busca específica
        Task<(List<UsuarioModel> usuarios, int total)> BuscarComFiltrosAsync(
            string? nome,
            string? email,
            string? cpf,
            string? PerfilUser,
            DateTime? dataInicio,
            DateTime? dataFim,
            int pagina,
            int itensPorPagina,
            string ordenarPor,
            string direcaoOrdem
        );

    }
}
