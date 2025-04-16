namespace AutoPecas.Core.Entities;

public class Venda
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public int IdCliente { get; set; }
    public Cliente Cliente { get; set; }
    public int IdVendedor { get; set; }
    public Vendedor Vendedor { get; set; }
    public List<VendaItem> Itens { get; set; } = new List<VendaItem>();
    public decimal ValorTotal { get; set; }
}