namespace AutoPecas.Core.Entities;

public class Veiculo
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Marca { get; set; }
    public string AnoModelo { get; set; }
    public List<Produto> ProdutosCompativeis { get; set; } = new List<Produto>();
}