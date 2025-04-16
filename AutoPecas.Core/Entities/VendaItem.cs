namespace AutoPecas.Core.Entities;

public class VendaItem
{
    public int Id { get; set; }
    public int IdVenda { get; set; }
    public Venda Venda { get; set; }
    public int IdProduto { get; set; }
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}