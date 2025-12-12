
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROJETOMVC.Repositorio;
namespace PROJETOMVC.Controllers
{
    [Authorize(Roles = "Administrador,Treinador")] 
    public class DashboardController : Controller
    {
        private readonly IDashboardRepositorio _svc;

        public DashboardController(IDashboardRepositorio svc) => _svc = svc;

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet("dashboard/metrics/resumo")]
        public async Task<IActionResult> Resumo() => Ok(await _svc.ObterResumoAsync());

        [HttpGet("dashboard/metrics/usuarios-por-mes")]
        public async Task<IActionResult> UsuariosPorMes(int meses = 12)
            => Ok(await _svc.UsuariosPorMesAsync(meses));

        [HttpGet("dashboard/metrics/treinos-por-dia")]
        public async Task<IActionResult> TreinosPorDia(int dias = 30)
            => Ok(await _svc.TreinosPorDiaAsync(dias));

        [HttpGet("dashboard/metrics/exercicios-mais-usados")]
        public async Task<IActionResult> ExerciciosMaisUsados(int top = 10)
            => Ok(await _svc.ExerciciosMaisUsadosAsync(top));

        [HttpGet("dashboard/metrics/adesao-ultimos7dias")]
        public async Task<IActionResult> AdesaoUltimos7Dias()
            => Ok(await _svc.AdesaoTreinosUltimos7DiasAsync());
    }
}
