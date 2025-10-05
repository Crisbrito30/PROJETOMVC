namespace PROJETOMVC.Models
{
    public class ContatoModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Email { get; set; }
        public string? Sexo { get; set; }
        public DateOnly? Nascimento { get; set; }
        public string? Telefone { get; set; }
        public string? Cpf { get; set; }
        public string? Permissao { get; set; }
    }
}