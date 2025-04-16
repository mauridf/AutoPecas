namespace AutoPecas.Core.Entities;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int QuantidadeEstoque { get; set; }
    public int QuantidadeMinima { get; set; }
    public string? Imagem { get; set; }
    public int IdFornecedor { get; set; }
    public Fornecedor Fornecedor { get; set; }
    public List<Veiculo> VeiculosCompativeis { get; set; } = new List<Veiculo>();
}