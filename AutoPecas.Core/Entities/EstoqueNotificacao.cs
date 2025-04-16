namespace AutoPecas.Core.Entities;

public class EstoqueNotificacao
{
    public int Id { get; set; }
    public int IdProduto { get; set; }
    public Produto Produto { get; set; }
    public int QuantidadeAtual { get; set; }
    public DateTime DataNotificacao { get; set; }
    public bool Resolvido { get; set; }
    public DateTime? DataResolucao { get; set; }
}