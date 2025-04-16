using System.ComponentModel.DataAnnotations;

namespace AutoPecas.Core.DTOs
{
    public class VendaDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Cliente é obrigatório")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Vendedor é obrigatório")]
        public int IdVendedor { get; set; }

        [Required(ErrorMessage = "Itens são obrigatórios")]
        [MinLength(1, ErrorMessage = "Deve haver pelo menos 1 item na venda")]
        public List<VendaItemDto> Itens { get; set; }
    }

    public class VendaItemDto
    {
        [Required(ErrorMessage = "Produto é obrigatório")]
        public int IdProduto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser pelo menos 1")]
        public int Quantidade { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Preço unitário deve ser maior que zero")]
        public decimal PrecoUnitario { get; set; }
    }
}