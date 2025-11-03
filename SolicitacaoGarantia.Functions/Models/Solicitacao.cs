namespace SolicitacaoGarantia.Functions.Models;

public class Solicitacao
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string DataNascimento { get; set; } = string.Empty;
    public string Celular { get; set; } = string.Empty;

    public InfoAparelho infoAparelho { get; set; } = new();

    public class InfoAparelho
    {
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string NotaFiscal { get; set; } = string.Empty;
        public string DataCompra { get; set; } = string.Empty;
        public string TipoDefeito { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
