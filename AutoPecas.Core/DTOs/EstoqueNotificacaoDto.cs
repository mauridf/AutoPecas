using System.ComponentModel.DataAnnotations;

namespace AutoPecas.Core.DTOs;

public class EstoqueNotificacaoDto
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Produto é obrigatório")]
    public int IdProduto { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
    public int QuantidadeAtual { get; set; }

    public bool Resolvido { get; set; }
}

public class EstoqueNotificacaoRespostaDto
{
    public int Id { get; set; }
    public int IdProduto { get; set; }
    public string ProdutoNome { get; set; }
    public int QuantidadeAtual { get; set; }
    public int QuantidadeMinima { get; set; }
    public DateTime DataNotificacao { get; set; }
    public bool Resolvido { get; set; }
    public DateTime? DataResolucao { get; set; }
}