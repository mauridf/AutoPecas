using AutoPecas.Core.DTOs;
using AutoPecas.Core.Interfaces;
using AutoPecas.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoPecas.Core.Exceptions;

namespace AutoPecas.API.Controllers;

/// <summary>
/// Controller para gerenciamento de fornecedores
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class FornecedorController : BaseController
{
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly ILogger<FornecedorController> _logger;

    public FornecedorController(
        IFornecedorRepository fornecedorRepository,
        ILogger<FornecedorController> logger)
    {
        _fornecedorRepository = fornecedorRepository;
        _logger = logger;
    }

    /// <summary>
    /// Listar Fornecedores
    /// </summary>
    /// <returns>Listar Fornecedores cadastrados</returns>
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] PaginationDto pagination)
    {
        try
        {
            var fornecedores = await _fornecedorRepository.Listar();
            return HandleResult(fornecedores);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar fornecedores");
            return HandleError("Erro ao listar fornecedores");
        }
    }

    /// <summary>
    /// Obter Fornecedor
    /// </summary>
    /// <returns>Obter Fornecedor pelo Id</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Obter(int id)
    {
        try
        {
            var fornecedor = await _fornecedorRepository.Obter(id);
            return HandleResult(fornecedor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter fornecedor {FornecedorId}", id);
            return HandleError($"Erro ao obter fornecedor {id}");
        }
    }

    /// <summary>
    /// Buscar Por Nome
    /// </summary>
    /// <returns>Buscar Fornecedor pelo Nome</returns>
    [HttpGet("buscar")]
    public async Task<IActionResult> BuscarPorNome([FromQuery] string nome)
    {
        try
        {
            var fornecedores = await _fornecedorRepository.BuscarPorNome(nome);
            return HandleResult(fornecedores);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar fornecedores por nome {Nome}", nome);
            return HandleError($"Erro ao buscar fornecedores por nome {nome}");
        }
    }

    /// <summary>
    /// Adicionar Fornecedor
    /// </summary>
    /// <returns>Adicionar/Registrar novo Fornecedor</returns>
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] FornecedorDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return HandleError("Dados inválidos", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            var fornecedor = new Fornecedor
            {
                Nome = dto.Nome,
                Contato = dto.Contato,
                Telefone = dto.Telefone,
                Email = dto.Email
            };

            await _fornecedorRepository.Adicionar(fornecedor);
            return HandleResult(fornecedor, "Fornecedor adicionado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar fornecedor");
            return HandleError("Erro ao adicionar fornecedor");
        }
    }

    /// <summary>
    /// Atualizar Fornecedor
    /// </summary>
    /// <returns>Atualizar informações do Fornecedor</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] FornecedorDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return HandleError("Dados inválidos", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            var fornecedor = await _fornecedorRepository.Obter(id);
            if (fornecedor == null)
                return HandleError("Fornecedor não encontrado");

            fornecedor.Nome = dto.Nome;
            fornecedor.Contato = dto.Contato;
            fornecedor.Telefone = dto.Telefone;
            fornecedor.Email = dto.Email;

            await _fornecedorRepository.Atualizar(fornecedor);
            return HandleResult(fornecedor, "Fornecedor atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar fornecedor {FornecedorId}", id);
            return HandleError($"Erro ao atualizar fornecedor {id}");
        }
    }

    /// <summary>
    /// Remover Fornecedor
    /// </summary>
    /// <returns>Remover Fornecedor</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            var fornecedor = await _fornecedorRepository.Obter(id);
            if (fornecedor == null)
                return HandleError("Fornecedor não encontrado");

            await _fornecedorRepository.Remover(fornecedor);
            return HandleResult(true, "Fornecedor removido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover fornecedor {FornecedorId}", id);
            return HandleError($"Erro ao remover fornecedor {id}");
        }
    }
}
