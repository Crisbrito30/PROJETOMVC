using PROJETOMVC.Models;
using PROJETOMVC.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaApp.Models
{
    public class Treino
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do treino é obrigatório")]
        [StringLength(100)]
        [Display(Name = "Nome do Treino")]
        public string Nome { get; set; }

        [StringLength(500)]
        [Display(Name = "Descrição/Objetivo")]
        public string Descricao { get; set; }

        [Required]
        [Display(Name = "Nível de Dificuldade")]
        public NivelDificuldade Nivel { get; set; }

        [Required]
        [Display(Name = "Tipo de Treino")]
        public TipoTreino Tipo { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Display(Name = "Data de Início")]
        public DateTime? DataInicio { get; set; }

        [Display(Name = "Data de Fim")]
        public DateTime? DataFim { get; set; }

        [Display(Name = "Status")]
        public bool Ativo { get; set; } = true;

        // Relacionamento com Usuario (ALUNO que vai fazer o treino)
        [Display(Name = "Aluno")]
        public int? UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }

        // Relacionamento com Usuario (TREINADOR que criou o treino)
        [Display(Name = "Criado por")]
        public int? CriadoPorId { get; set; }

        [ForeignKey("CriadoPorId")]
        public virtual Usuario? CriadoPor { get; set; }

        // Relacionamento com os exercícios do treino
        public virtual ICollection<TreinoExercicio>? TreinoExercicios { get; set; }

        // Relacionamento com execuções
        public virtual ICollection<ExecucaoTreino>? Execucoes { get; set; }
    }
}