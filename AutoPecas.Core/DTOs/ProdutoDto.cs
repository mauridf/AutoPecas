using System.ComponentModel.DataAnnotations;

namespace AutoPecas.Core.DTOs;

public class ProdutoDto
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }

    [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
    public string Descricao { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
    public int QuantidadeEstoque { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantidade mínima deve ser pelo menos 1")]
    public int QuantidadeMinima { get; set; }

    public string? Imagem { get; set; }

    [Required(ErrorMessage = "Fornecedor é obrigatório")]
    public int IdFornecedor { get; set; }

    public List<int>? IdsVeiculosCompativeis { get; set; }
}