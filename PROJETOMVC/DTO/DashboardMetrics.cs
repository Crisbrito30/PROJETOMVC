
namespace PROJETOMVC.DTO
{
    public class DashboardMetrics
    {
        public IEnumerable<SerieTemporal> UsuariosPorMes { get; set; } = [];
        public IEnumerable<SerieTemporal> TreinosPorDia { get; set; } = [];
        public IEnumerable<ItemQuantidade> ExerciciosMaisUsados { get; set; } = [];
        public IEnumerable<ItemQuantidade> AdesaoTreinosUltimos7Dias { get; set; } = [];
        public int TotalUsuarios { get; set; }
        public int TotalTreinos { get; set; }
        public int TotalExercicios { get; set; }
    }

    public class SerieTemporal
    {
        public string Label { get; set; } = default!;
        public int Valor { get; set; }
    }

    public class ItemQuantidade
    {
        public string Nome { get; set; } = default!;
        public int Quantidade { get; set; }
    }
}
