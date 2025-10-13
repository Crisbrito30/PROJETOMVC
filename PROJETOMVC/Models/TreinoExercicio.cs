using AcademiaApp.Models;
using PROJETOMVC.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROJETOMVC.Models
{
    // Model de Relacionamento entre Treino e Exercício
    public class TreinoExercicio
    {
        [Key]
        public int Id { get; set; }

        // Relacionamento com Treino
        [Required]
        public int TreinoId { get; set; }

        [ForeignKey("TreinoId")]
        public virtual Treino Treino { get; set; }

        // Relacionamento com Exercício
        [Required]
        public int ExercicioId { get; set; }

        [ForeignKey("ExercicioId")]
        public virtual Exercicio Exercicio { get; set; }

        // Informações específicas do exercício no treino
        [Required]
        [Display(Name = "Ordem de Execução")]
        public int Ordem { get; set; }

        [Required]
        [Display(Name = "Séries")]
        [Range(1, 20)]
        public int Series { get; set; }

        [Required]
        [Display(Name = "Repetições")]
        [StringLength(20)]
        public string Repeticoes { get; set; } // Ex: "12", "10-12", "máximo"

        [Display(Name = "Carga (kg)")]
        public decimal? Carga { get; set; }

        [Display(Name = "Descanso (segundos)")]
        [Range(0, 600)]
        public int? TempoDescanso { get; set; }

        [StringLength(200)]
        [Display(Name = "Observações")]
        public string Observacoes { get; set; }

        [Display(Name = "Dia da Semana")]
        public DiaSemana? DiaSemana { get; set; }

        [StringLength(10)]
        [Display(Name = "Divisão (A/B/C)")]
        public string Divisao { get; set; } // Ex: "A", "B", "C"
    }
}
