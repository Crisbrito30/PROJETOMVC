using Microsoft.AspNetCore.Mvc;
using PROJETOMVC.Models;
using PROJETOMVC.Repositorio;
using Microsoft.AspNetCore.Authorization;
using AcademiaApp.Models;
namespace PROJETOMVC.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        // GET: Usuario/Index - Lista todos os usuários  com filtros e paginação
        public async Task<IActionResult> Index(UsuarioFiltroViewModel filtro)
        {
            try
            {
                // Define valores padrão se não informados
                filtro.PaginaAtual = filtro.PaginaAtual <= 0 ? 1 : filtro.PaginaAtual;
                filtro.ItensPorPagina = filtro.ItensPorPagina <= 0 ? 10 : filtro.ItensPorPagina;
                filtro.OrdenarPor ??= "Nome";
                filtro.ordernarDirecao ??= "asc";

                // Busca com filtros
                var (usuarios, total) = await _usuarioRepositorio.BuscarComFiltrosAsync(
                 filtro.Nome,
                 filtro.Email,
                 filtro.Cpf,
                 filtro.PerfilUser,        // ✅ Ajuste se necessário
                 filtro.dataInical,        // ✅ Ajuste se necessário
                 filtro.dataFinal,         // ✅ Ajuste se necessário
                 filtro.PaginaAtual,
                 filtro.ItensPorPagina,
                 filtro.OrdenarPor,
                 filtro.ordernarDirecao    // ✅ Ajuste se necessário
 );

                filtro.Usuarios = usuarios;
                filtro.TotalItens = total;

                return View(filtro);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar usuários: {ex.Message}";
                return View(new UsuarioFiltroViewModel());
            }
        }

        // POST: Usuario/Adicionar - Cria novo usuário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _usuarioRepositorio.AdicionarAsync(usuario);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
                }
                else
                {

                    TempData["MensagemErro"] = "Erro: Campos obrigatórios não preenchidos!";
                }
            }
            catch (Exception ex)
            {
                var mensagem = ex.Message;

                //Trata erros específicos de CPF ou Email já cadastrado
                if (ex.InnerException?.Message?.Contains("IX_Usuarios_Cpf") == true)
                {
                    mensagem = "CPF já cadastrado no sistema!";
                }
                else if (ex.InnerException?.Message?.Contains("IX_Usuarios_Email") == true)
                {
                    mensagem = "Email já cadastrado no sistema!";
                }
                var inner = ex.InnerException?.Message ?? "Sem detalhes internos";
                TempData["MensagemErro"] = $"Erro ao cadastrar usuário: {ex.Message} → Detalhes: {inner}";
            }

            return RedirectToAction("Index");
        }

        // POST: Usuario/Atualizar - Atualiza usuário existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atualizar(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ModelState.Remove("Senha");

                    await _usuarioRepositorio.AtualizarAsync(usuario);
                    TempData["MensagemSucesso"] = "Usuário atualizado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Preencha todos os campos obrigatórios.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro detalhado: {ex.InnerException?.Message}");
                TempData["MensagemErro"] = $"Erro ao atualizar usuário: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // POST: Usuario/Deletar - Deleta usuário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                var usuarioDeletado = await _usuarioRepositorio.DeletarAsync(id);

                if (usuarioDeletado)
                {
                    TempData["MensagemSucesso"] = "Usuário excluído com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Usuário não encontrado.";
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao excluir usuário: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // GET: Usuario/Detalhes/5 - Visualiza detalhes (opcional)
        public async Task<IActionResult> Detalhes(int id)
        {
            try
            {
                var usuario = await _usuarioRepositorio.ListarPorIdAsync(id);

                if (usuario == null)
                {
                    TempData["MensagemErro"] = "Usuário não encontrado.";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar usuário: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}