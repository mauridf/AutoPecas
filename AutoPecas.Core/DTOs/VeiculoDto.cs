using System.ComponentModel.DataAnnotations;

namespace AutoPecas.Core.DTOs;

public class VeiculoDto
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Marca é obrigatória")]
    [StringLength(50, ErrorMessage = "Marca deve ter no máximo 50 caracteres")]
    public string Marca { get; set; }

    [Required(ErrorMessage = "Ano/Modelo é obrigatório")]
    [StringLength(20, ErrorMessage = "Ano/Modelo deve ter no máximo 20 caracteres")]
    public string AnoModelo { get; set; }
}