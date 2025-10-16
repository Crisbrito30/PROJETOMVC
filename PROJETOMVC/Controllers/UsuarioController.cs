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

        // GET: Usuario/Index - Lista todos os usuários
        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarios = await _usuarioRepositorio.BuscarTodosAsync();
                return View(usuarios);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar usuários: {ex.Message}";
                return View(new List<Usuario>());
            }
        }

        // POST: Usuario/Adicionar - Cria novo usuário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar(Usuario usuario)
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
                var inner = ex.InnerException?.Message ?? "Sem detalhes internos";
                TempData["MensagemErro"] = $"Erro ao cadastrar usuário: {ex.Message} → Detalhes: {inner}";
            }

            return RedirectToAction("Index");
        }

        // POST: Usuario/Atualizar - Atualiza usuário existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atualizar(Usuario usuario)
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