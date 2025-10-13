using AcademiaApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROJETOMVC.Models
{
    // Model para detalhar a execução de cada exercício
    public class ExecucaoExercicio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ExecucaoTreinoId { get; set; }

        [ForeignKey("ExecucaoTreinoId")]
        public virtual ExecucaoTreino ExecucaoTreino { get; set; }

        [Required]
        public int TreinoExercicioId { get; set; }

        [ForeignKey("TreinoExercicioId")]
        public virtual TreinoExercicio TreinoExercicio { get; set; }

        [Display(Name = "Carga Utilizada (kg)")]
        public decimal? CargaUtilizada { get; set; }

        [Display(Name = "Repetições Realizadas")]
        public int? RepeticoesRealizadas { get; set; }

        [Display(Name = "Executado")]
        public bool Executado { get; set; } = false;

        [StringLength(200)]
        [Display(Name = "Observações")]
        public string Observacoes { get; set; }
    }
}

