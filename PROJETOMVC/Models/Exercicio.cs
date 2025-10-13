using PROJETOMVC.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PROJETOMVC.Models
{
    // Model para o Catálogo de Exercícios
    public class Exercicio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do exercício é obrigatório")]
        [StringLength(100)]
        [Display(Name = "Nome do Exercício")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Grupo Muscular")]
        public GrupoMuscular GrupoMuscular { get; set; }

        [StringLength(500)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [StringLength(200)]
        [Display(Name = "Link do Vídeo")]
        public string? VideoUrl { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        // Relacionamento
        public virtual ICollection<TreinoExercicio>? TreinoExercicios { get; set; }
    }
}
