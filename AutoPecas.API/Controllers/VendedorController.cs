using AutoPecas.Core.DTOs;
using AutoPecas.Core.Interfaces;
using AutoPecas.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoPecas.Core.Exceptions;

namespace AutoPecas.API.Controllers;

/// <summary>
/// Controller para gerenciamento de vendedores
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class VendedorController : BaseController
{
    private readonly IVendedorRepository _vendedorRepository;
    private readonly ILogger<VendedorController> _logger;

    public VendedorController(
        IVendedorRepository vendedorRepository,
        ILogger<VendedorController> logger)
    {
        _vendedorRepository = vendedorRepository;
        _logger = logger;
    }

    /// <summary>
    /// Listar Vendedores
    /// </summary>
    /// <returns>Listar Vendedores cadastrados</returns>
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] PaginationDto pagination)
    {
        try
        {
            var vendedores = await _vendedorRepository.Listar();
            return HandleResult(vendedores);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar vendedores");
            return HandleError("Erro ao listar vendedores");
        }
    }

    /// <summary>
    /// Obter Vendedor
    /// </summary>
    /// <returns>Obter Vendedor pelo Id</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Obter(int id)
    {
        try
        {
            var vendedor = await _vendedorRepository.Obter(id);
            return HandleResult(vendedor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter vendedor {VendedorId}", id);
            return HandleError($"Erro ao obter vendedor {id}");
        }
    }

    /// <summary>
    /// Obter Por Email
    /// </summary>
    /// <returns>Obter Vendedor pelo E-mail</returns>
    [HttpGet("email/{email}")]
    public async Task<IActionResult> ObterPorEmail(string email)
    {
        try
        {
            var vendedor = await _vendedorRepository.ObterPorEmail(email);
            return HandleResult(vendedor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter vendedor por email {Email}", email);
            return HandleError($"Erro ao obter vendedor por email {email}");
        }
    }

    /// <summary>
    /// Adicionar Vendedor
    /// </summary>
    /// <returns>Cadastrar Vendedor</returns>
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] VendedorDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return HandleError("Dados inválidos", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            var vendedor = new Vendedor
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Telefone = dto.Telefone
            };

            await _vendedorRepository.Adicionar(vendedor);
            return HandleResult(vendedor, "Vendedor adicionado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar vendedor");
            return HandleError("Erro ao adicionar vendedor");
        }
    }

    /// <summary>
    /// Atualizar Vendedor
    /// </summary>
    /// <returns>Atualizar informações do Vendedor</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] VendedorDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return HandleError("Dados inválidos", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            var vendedor = await _vendedorRepository.Obter(id);
            if (vendedor == null)
                return HandleError("Vendedor não encontrado");

            vendedor.Nome = dto.Nome;
            vendedor.Email = dto.Email;
            vendedor.Telefone = dto.Telefone;

            await _vendedorRepository.Atualizar(vendedor);
            return HandleResult(vendedor, "Vendedor atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar vendedor {VendedorId}", id);
            return HandleError($"Erro ao atualizar vendedor {id}");
        }
    }

    /// <summary>
    /// Remover Vendedor
    /// </summary>
    /// <returns>Excluir Vendedor</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            var vendedor = await _vendedorRepository.Obter(id);
            if (vendedor == null)
                return HandleError("Vendedor não encontrado");

            await _vendedorRepository.Remover(vendedor);
            return HandleResult(true, "Vendedor removido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover vendedor {VendedorId}", id);
            return HandleError($"Erro ao remover vendedor {id}");
        }
    }
}
