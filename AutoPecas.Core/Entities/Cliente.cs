namespace AutoPecas.Core.Entities;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Documento { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public List<Venda> Vendas { get; set; } = new List<Venda>();
}