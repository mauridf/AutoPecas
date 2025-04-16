namespace AutoPecas.Core.Entities;

public class Fornecedor
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Contato { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public List<Produto> Produtos { get; set; } = new List<Produto>();
}