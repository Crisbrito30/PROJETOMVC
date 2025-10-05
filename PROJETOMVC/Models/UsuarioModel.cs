using PROJETOMVC.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace PROJETOMVC.Models
{
    public class UsuarioModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Login é obrigatório")]
        [StringLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Perfil é obrigatório")]
        public PerfilEnum Perfil { get; set; } = PerfilEnum.Padrao;

        [StringLength(255)]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public DateTime DataCadastro { get; set; }= DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }
    }
}