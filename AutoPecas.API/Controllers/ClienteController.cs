using AutoPecas.Core.DTOs;
using AutoPecas.Core.Interfaces;
using AutoPecas.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoPecas.Core.Exceptions;
using AutoPecas.Core.Services;

namespace AutoPecas.API.Controllers;

/// <summary>
/// Controller para gerenciamento de clientes
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ClienteController : BaseController
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IClienteService _clienteService;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(
        IClienteRepository clienteRepository,
        IClienteService clienteService,
        ILogger<ClienteController> logger)
    {
        _clienteRepository = clienteRepository;
        _clienteService = clienteService;
        _logger = logger;
    }

    /// <summary>
    /// Listar Clientes
    /// </summary>
    /// <returns>Listar Clientes cadastrados</returns>
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] PaginationDto pagination)
    {
        try
        {
            var clientes = await _clienteRepository.Listar();
            return HandleResult(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar clientes");
            return HandleError("Erro ao listar clientes");
        }
    }

    /// <summary>
    /// Obter Cliente
    /// </summary>
    /// <returns>Obter Cliente pelo Id</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Obter(int id)
    {
        try
        {
            var cliente = await _clienteRepository.Obter(id);
            return HandleResult(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter cliente {ClienteId}", id);
            return HandleError($"Erro ao obter cliente {id}");
        }
    }

    /// <summary>
    /// Obter Cliente pelo Documento
    /// </summary>
    /// <returns>Obter o Cliente pelo Documento</returns>
    [HttpGet("documento/{documento}")]
    public async Task<IActionResult> ObterPorDocumento(string documento)
    {
        try
        {
            var cliente = await _clienteRepository.ObterPorDocumento(documento);
            return HandleResult(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter cliente por documento {Documento}", documento);
            return HandleError($"Erro ao obter cliente por documento {documento}");
        }
    }

    /// <summary>
    /// Adicionar Cliente
    /// </summary>
    /// <returns>Adicionar/Registrar um novo Cliente</returns>
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] ClienteDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return HandleError("Dados inválidos", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            await _clienteService.ValidarDocumentoExistente(dto.Documento);

            var cliente = new Cliente
            {
                Nome = dto.Nome,
                Documento = dto.Documento,
                Email = dto.Email,
                Telefone = dto.Telefone
            };

            await _clienteRepository.Adicionar(cliente);
            return HandleResult(cliente, "Cliente adicionado com sucesso");
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, "Erro de negócio ao adicionar cliente");
            return HandleError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar cliente");
            return HandleError("Erro ao adicionar cliente");
        }
    }

    /// <summary>
    /// Atualizar Cliente
    /// </summary>
    /// <returns>Atualizar informações do Cliente</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] ClienteDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return HandleError("Dados inválidos", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            var cliente = await _clienteRepository.Obter(id);
            if (cliente == null)
                return HandleError("Cliente não encontrado");

            if (cliente.Documento != dto.Documento)
                await _clienteService.ValidarDocumentoExistente(dto.Documento);

            cliente.Nome = dto.Nome;
            cliente.Documento = dto.Documento;
            cliente.Email = dto.Email;
            cliente.Telefone = dto.Telefone;

            await _clienteRepository.Atualizar(cliente);
            return HandleResult(cliente, "Cliente atualizado com sucesso");
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, "Erro de negócio ao atualizar cliente {ClienteId}", id);
            return HandleError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar cliente {ClienteId}", id);
            return HandleError($"Erro ao atualizar cliente {id}");
        }
    }

    /// <summary>
    /// Deletar Cliente
    /// </summary>
    /// <returns>Excluir Cliente</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            var cliente = await _clienteRepository.Obter(id);
            if (cliente == null)
                return HandleError("Cliente não encontrado");

            await _clienteRepository.Remover(cliente);
            return HandleResult(true, "Cliente removido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover cliente {ClienteId}", id);
            return HandleError($"Erro ao remover cliente {id}");
        }
    }
}
