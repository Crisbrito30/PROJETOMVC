using System.ComponentModel.DataAnnotations;

namespace PROJETOMVC.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Login é obrigatório")]
        [StringLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;
    }
}