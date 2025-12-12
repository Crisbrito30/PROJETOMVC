
using PROJETOMVC.DTO;

namespace PROJETOMVC.Repositorio
{
    public interface IDashboardRepositorio
    {
        Task<DashboardMetrics> ObterResumoAsync(DateTime? inicio = null, DateTime? fim = null);
        Task<IEnumerable<SerieTemporal>> UsuariosPorMesAsync(int meses = 12);
        Task<IEnumerable<SerieTemporal>> TreinosPorDiaAsync(int dias = 30);
        Task<IEnumerable<ItemQuantidade>> ExerciciosMaisUsadosAsync(int top = 10);
        Task<IEnumerable<ItemQuantidade>> AdesaoTreinosUltimos7DiasAsync();
    }
}
