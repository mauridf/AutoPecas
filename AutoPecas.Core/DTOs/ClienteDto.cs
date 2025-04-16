using System.ComponentModel.DataAnnotations;

namespace AutoPecas.Core.DTOs;

public class ClienteDto
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Documento é obrigatório")]
    [StringLength(20, ErrorMessage = "Documento deve ter no máximo 20 caracteres")]
    public string Documento { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
    public string? Email { get; set; }

    [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
    public string? Telefone { get; set; }
}