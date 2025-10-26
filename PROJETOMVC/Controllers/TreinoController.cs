using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PROJETOMVC.Repositorio;
using AcademiaApp.Models;
using System;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PROJETOMVC.Controllers
{
    [Authorize]
    public class TreinoController : Controller
    {
        private readonly ITreinoRepositorio _treinoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public TreinoController(ITreinoRepositorio treinoRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _treinoRepositorio = treinoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }

        // GET: Treino/Index - Lista todos os treinos
        public async Task<IActionResult> Index()
        {
            try
            {
                var treinos = await _treinoRepositorio.BuscarTodosAsync();
                return View(treinos);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar treinos: {ex.Message}";
                return View(new List<Treino>());
            }
        }

        // GET: Treino/MeusTreinos - Treinos do aluno logado
        public async Task<IActionResult> MeusTreinos()
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var treinos = await _treinoRepositorio.BuscarPorUsuarioAsync(usuarioId);
                return View(treinos);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar seus treinos: {ex.Message}";
                return View(new List<Treino>());
            }
        }

        // GET: Treino/Detalhes/5
        public async Task<IActionResult> Detalhes(int id)
        {
            try
            {
                var treino = await _treinoRepositorio.BuscarComExerciciosAsync(id);

                if (treino == null)
                {
                    TempData["MensagemErro"] = "Treino não encontrado.";
                    return RedirectToAction("Index");
                }

                return View(treino);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar treino: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: Treino/Adicionar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar(Treino treino)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Define quem criou o treino (usuário logado)
                    var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                    treino.CriadoPorId = usuarioId;

                    await _treinoRepositorio.AdicionarAsync(treino);
                    TempData["MensagemSucesso"] = "Treino cadastrado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Erro: Campos obrigatórios não preenchidos!";
                }
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "Sem detalhes internos";
                TempData["MensagemErro"] = $"Erro ao cadastrar treino: {ex.Message} → Detalhes: {inner}";
            }

            return RedirectToAction("Index");
        }

        // POST: Treino/Atualizar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atualizar(Treino treino)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _treinoRepositorio.AtualizarAsync(treino);
                    TempData["MensagemSucesso"] = "Treino atualizado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Preencha todos os campos obrigatórios.";
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao atualizar treino: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // POST: Treino/Deletar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                var treinoDeletado = await _treinoRepositorio.DeletarAsync(id);

                if (treinoDeletado)
                {
                    TempData["MensagemSucesso"] = "Treino excluído com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Treino não encontrado.";
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao excluir treino: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // Método auxiliar para popular dropdowns
        private async Task CarregarViewBags()
        {
            var usuarios = await _usuarioRepositorio.BuscarTodosAsync();
            ViewBag.Usuarios = usuarios;
        }
    }
}