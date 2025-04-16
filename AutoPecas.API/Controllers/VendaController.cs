using AutoPecas.Core.DTOs;
using AutoPecas.Core.Interfaces;
using AutoPecas.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoPecas.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AutoPecas.API.Controllers;

/// <summary>
/// Controller para gerenciamento de vendas
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VendaController : BaseController
{
    private readonly IVendaRepository _vendaRepository;
    private readonly IEstoqueService _estoqueService;
    private readonly ILogger<VendaController> _logger;

    public VendaController(
        IVendaRepository vendaRepository,
        IEstoqueService estoqueService,
        ILogger<VendaController> logger)
    {
        _vendaRepository = vendaRepository;
        _estoqueService = estoqueService;
        _logger = logger;
    }

    /// <summary>
    /// Registrar Venda
    /// </summary>
    /// <returns>Registro da Venda</returns>
    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] VendaDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return HandleError("Dados inválidos", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            var venda = new Venda
            {
                Data = DateTime.Now,
                IdCliente = dto.IdCliente,
                IdVendedor = dto.IdVendedor,
                Itens = dto.Itens.Select(i => new VendaItem
                {
                    IdProduto = i.IdProduto,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    Subtotal = i.Quantidade * i.PrecoUnitario
                }).ToList(),
                ValorTotal = dto.Itens.Sum(i => i.Quantidade * i.PrecoUnitario)
            };

            // Atualizar estoque para cada item
            foreach (var item in venda.Itens)
            {
                await _estoqueService.AtualizarEstoque(item.IdProduto, -item.Quantidade);
            }

            await _vendaRepository.Adicionar(venda);
            return HandleResult(venda, "Venda registrada com sucesso");
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, "Erro de negócio ao registrar venda");
            return HandleError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar venda");
            return HandleError("Erro ao registrar venda");
        }
    }

    /// <summary>
    /// Obter Venda
    /// </summary>
    /// <returns>Buscar a Venda efetuada pelo Id</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Obter(int id)
    {
        try
        {
            var venda = await _vendaRepository.ObterComItens(id);
            return HandleResult(venda);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter venda {VendaId}", id);
            return HandleError($"Erro ao obter venda {id}");
        }
    }

    /// <summary>
    /// Vendas por Período
    /// </summary>
    /// <returns>Listar Vendas por período informado</returns>
    [HttpGet("periodo")]
    public async Task<IActionResult> ListarPorPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        try
        {
            var vendas = await _vendaRepository.ListarPorPeriodo(inicio, fim);
            return HandleResult(vendas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar vendas por período");
            return HandleError("Erro ao listar vendas por período");
        }
    }
}