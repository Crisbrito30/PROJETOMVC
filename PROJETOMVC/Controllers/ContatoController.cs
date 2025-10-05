using Microsoft.AspNetCore.Mvc;
using PROJETOMVC.Models;
using PROJETOMVC.Repositorio;

namespace PROJETOMVC.Controllers
{
    public class ContatoController : Controller
    {
        private readonly IContatoRepositorio _contatoRepositorio;

        public ContatoController(IContatoRepositorio contatoRepositorio)
        {
            _contatoRepositorio = contatoRepositorio;
        }

        // ✅ Listar contatos
        public async Task<IActionResult> Index()
        {
            var contatos = await _contatoRepositorio.BuscarTodosAsync();
            return View(contatos);
        }

        // ✅ Criar (GET)
        public IActionResult Criar()
        {
            return View();
        }

        // ✅ Criar (POST)
        [HttpPost]
        public async Task<IActionResult> Criar(ContatoModel contato)
        {
            if (ModelState.IsValid)
            {
                await _contatoRepositorio.AdicionarAsync(contato);
                return RedirectToAction("Index");
            }
            return View(contato);
        }

        // ✅ Editar (GET)
        public async Task<IActionResult> Editar(int id)
        {
            var contato = await _contatoRepositorio.ListarPorIdAsync(id);
            if (contato == null) return NotFound();
            return View(contato);
        }

        // ✅ Editar (POST)
        [HttpPost]
        public async Task<IActionResult> Editar(ContatoModel contato)
        {
            if (ModelState.IsValid)
            {
                await _contatoRepositorio.AtualizarAsync(contato);
                return RedirectToAction("Index");
            }
            return View(contato);
        }

        // ✅ Detalhes
        public async Task<IActionResult> Detalhes(int id)
        {
            var contato = await _contatoRepositorio.ListarPorIdAsync(id);
            if (contato == null) return NotFound();
            return View(contato);
        }

        // ✅ Deletar (GET)
        public async Task<IActionResult> Deletar(int id)
        {
            var contato = await _contatoRepositorio.ListarPorIdAsync(id);
            if (contato == null) return NotFound();
            return View(contato);
        }

        // ✅ Deletar (POST)
        [HttpPost]
        public async Task<IActionResult> DeletarConfirmado(int id)
        {
            await _contatoRepositorio.DeletarAsync(id);
            return RedirectToAction("Index");
        }
    }
}
