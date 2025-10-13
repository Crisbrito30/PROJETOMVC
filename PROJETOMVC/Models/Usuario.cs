using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaApp.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100)]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(20)]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(14)]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }

        [StringLength(200)]
        [Display(Name = "Endereço")]
        public string? Endereco { get; set; }

        [Required(ErrorMessage = "O tipo de usuário é obrigatório")]
        [Display(Name = "Tipo de Usuário")]
        public TipoUsuario TipoUsuario { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        // Senha (você pode implementar hash depois)
        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        // Campos específicos para ALUNOS
        [Display(Name = "Data de Matrícula")]
        [DataType(DataType.Date)]
        public DateTime? DataMatricula { get; set; }

        [Display(Name = "Plano")]
        public string? Plano { get; set; }

        [StringLength(500)]
        [Display(Name = "Observações Médicas")]
        public string? ObservacoesMedicas { get; set; }

        // Campos específicos para TREINADORES
        [StringLength(50)]
        [Display(Name = "CREF")]
        public string? Cref { get; set; }

        [StringLength(100)]
        [Display(Name = "Especialidade")]
        public string? Especialidade { get; set; }

        // Relacionamentos quando o usuário é ALUNO
        public virtual ICollection<Treino> Treinos { get; set; }
        public virtual ICollection<ExecucaoTreino> ExecucoesTreino { get; set; }

        // Relacionamento quando o usuário é TREINADOR (treinos criados por ele)
        public virtual ICollection<Treino> TreinosCriados { get; set; }
    }

    public enum TipoUsuario
    {
        [Display(Name = "Aluno")]
        Aluno = 1,

        [Display(Name = "Treinador")]
        Treinador = 2,

        [Display(Name = "Administrador")]
        Administrador = 3
    }
}