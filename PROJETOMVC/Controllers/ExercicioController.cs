using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using PROJETOMVC.Models;
using PROJETOMVC.Models.Enums;
using PROJETOMVC.Repositorio;
using System.Threading.Tasks;



namespace PROJETOMVC.Controllers
{
    public class ExercicioController : Controller
    {
        private readonly IExercicioRepositorio _exercicioRepositorio;

        public ExercicioController(IExercicioRepositorio exercicioRepositorio)
        {
            _exercicioRepositorio = exercicioRepositorio;
        }


        // GET: Exercicio/Index - Lista todos os exercícios
        public async Task<IActionResult> Index(string? grupoMuscular)
        {
            try
            {
                IEnumerable<Exercicio> exercicios;

                if ((string.IsNullOrEmpty(grupoMuscular) && Enum.TryParse<GrupoMuscular>(grupoMuscular, out var grupo)))
                {
                    exercicios = await _exercicioRepositorio.BuscarPorGrupoMuscularAsync(grupo);
                    ViewBag.FiltroAtual = grupo.ToString();
                }
                else
                {
                    exercicios = await _exercicioRepositorio.BuscarTodosAsync();
                }

                return View(exercicios);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar exercícios: {ex.Message}";
                return View(new List<Exercicio>());
            }
        }
        //POST EXERCICIO ADICIONAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarA(Exercicio exercicio)
        {

            try
            {
                ModelState.Remove("TreinoExercicios");

                if (!ModelState.IsValid)
                {
                    await _exercicioRepositorio.AdicionarAsync(exercicio);
                    TempData["MensagemSucesso"] = "Exercício adicionado com sucesso!";

                }
                else
                {
                    TempData["MensagemErro"] = "Erro ao adicionar exercício. Verifique os dados informados.";

                }
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "sem detalhes adicionais";
                TempData["MensagemErro"] = $"Erro ao adicionar exercício: {ex.Message}";

            }
            return RedirectToAction("Index");
        }
        //POST EXERCICIO ATUALIZAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atualizar(Exercicio exercicio)
        {
            try
            {
                // Remove validação de propriedades de navegação
                ModelState.Remove("TreinoExercicios");

                if (ModelState.IsValid)
                {
                    await _exercicioRepositorio.AtualizarAsync(exercicio);
                    TempData["MensagemSucesso"] = "Exercício atualizado com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MensagemErro"] = "Erro ao atualizar exercício. Verifique os dados informados.";
                }
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "sem detalhes adicionais";
                TempData["MensagemErro"] = $"Erro ao atualizar exercício: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
        //POST EXERCICIO DELETAR
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                var exercicioDeletado = await _exercicioRepositorio.DeletarAsync(id);

                if (exercicioDeletado)
                {
                    TempData["MensagemSucesso"] = "Exercício deletado com sucesso!";


                }
                else
                {
                    TempData["MensagemErro"] = "Erro ao deletar exercício.";


                }
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "sem detalhes adicionais";
                TempData["MensagemErro"] = $"Erro ao deletar exercício: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
    }
}


