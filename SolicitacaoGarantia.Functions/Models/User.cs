namespace SolicitacaoGarantia.Functions.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public DateTime Data_Nascimento { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
    }
}
