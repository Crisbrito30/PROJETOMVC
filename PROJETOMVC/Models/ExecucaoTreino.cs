using PROJETOMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaApp.Models
{
    public class ExecucaoTreino
    {
        [Key]
        public int Id { get; set; }

        // Relacionamento com Treino
        [Required]
        public int TreinoId { get; set; }

        [ForeignKey("TreinoId")]
        public virtual Treino Treino { get; set; }

        // Relacionamento com Usuario (ALUNO)
        [Required]
        [Display(Name = "Aluno")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        [Required]
        [Display(Name = "Data da Execução")]
        public DateTime DataExecucao { get; set; } = DateTime.Now;

        [Display(Name = "Concluído")]
        public bool Concluido { get; set; } = false;

        [StringLength(500)]
        [Display(Name = "Observações do Aluno")]
        public string ObservacoesAluno { get; set; }

        // Detalhes da execução de cada exercício
        public virtual ICollection<ExecucaoExercicio> ExecucaoExercicios { get; set; }
    }
}