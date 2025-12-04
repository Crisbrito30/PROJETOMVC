using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PROJETOMVC.Repositorio;
using AcademiaApp.Models;
using AcademiaApp.Data;
using PROJETOMVC.Models;
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
        private readonly IExercicioRepositorio _exercicioRepositorio;
        private readonly AcademiaContext _context;

        public TreinoController(ITreinoRepositorio treinoRepositorio, IUsuarioRepositorio usuarioRepositorio, IExercicioRepositorio exercicioRepositorio, AcademiaContext context)
        {
            _treinoRepositorio = treinoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _exercicioRepositorio = exercicioRepositorio;
            _context = context;
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

                await CarregarViewBags();
                return View(treino);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar treino: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: Treino/AdicionarExercicio
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarExercicio(int TreinoId, int ExercicioId, int Ordem, int Series, string Repeticoes, decimal? Carga, int? TempoDescanso, string Observacoes, string? Divisao)
        {
            try
            {
                var erroValidacao = ValidarDadosTreinoExercicio(TreinoId, ExercicioId, Ordem, Series, Repeticoes);
                if (!string.IsNullOrEmpty(erroValidacao))
                {
                    TempData["MensagemErro"] = erroValidacao;
                    return RedirectToAction("Detalhes", new { id = TreinoId });
                }

                var treino = await _treinoRepositorio.BuscarPorIdAsync(TreinoId);
                var exercicio = await _exercicioRepositorio.BuscarPorIdAsync(ExercicioId);

                if (treino == null || exercicio == null)
                {
                    TempData["MensagemErro"] = "Treino ou exercício não encontrados.";
                    return RedirectToAction("Detalhes", new { id = TreinoId });
                }

                var treinoExercicio = new Models.TreinoExercicio
                {
                    TreinoId = TreinoId,
                    ExercicioId = ExercicioId,
                    Ordem = Ordem,
                    Series = Series,
                    Repeticoes = Repeticoes,
                    Carga = Carga,
                    TempoDescanso = TempoDescanso,
                    Observacoes = Observacoes ?? string.Empty,
                    Divisao = string.IsNullOrWhiteSpace(Divisao) ? null : Divisao
                };

                await _treinoRepositorio.AdicionarTreinoExercicioAsync(treinoExercicio);
                TempData["MensagemSucesso"] = "Exercício adicionado ao treino com sucesso.";
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message;
                string extra = ex.GetType().Name;
                if (!string.IsNullOrWhiteSpace(inner))
                    extra += ": " + inner;

                TempData["MensagemErro"] = $"Erro ao adicionar exercício: {ex.Message} → {extra}";
            }

            return RedirectToAction("Detalhes", new { id = TreinoId });
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
            
            // Exercícios ativos para dropdowns no modal
            var exercicios = await _exercicioRepositorio.BuscarAtivosAsync();
            ViewBag.Exercicios = exercicios;
        }

        // Validação simples para os campos do TreinoExercicio vindo do modal
        private string? ValidarDadosTreinoExercicio(int treinoId, int exercicioId, int ordem, int series, string repeticoes)
        {
            if (treinoId <= 0) return "Treino inválido.";
            if (exercicioId <= 0) return "Exercício inválido.";
            if (ordem <= 0) return "A ordem deve ser maior que zero.";
            if (series <= 0) return "O número de séries deve ser maior que zero.";
            if (string.IsNullOrWhiteSpace(repeticoes)) return "Preencha as repetições.";
            return null;
        }
    }
}