using AutoPecas.Core.DTOs;
using AutoPecas.Core.Interfaces;
using AutoPecas.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoPecas.Core.Exceptions;
using System.Net;
using AutoPecas.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoPecas.Infrastructure.Data;

namespace AutoPecas.API.Controllers;

/// <summary>
/// Controller para gerenciamento de produtos
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProdutoController : BaseController
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly AutoPecasDbContext _context;
    private readonly IEstoqueService _estoqueService;
    private readonly ILogger<ProdutoController> _logger;

    public ProdutoController(
        IProdutoRepository produtoRepository,
        IFornecedorRepository fornecedorRepository,
        AutoPecasDbContext context,
        IEstoqueService estoqueService,
        ILogger<ProdutoController> logger)
    {
        _produtoRepository = produtoRepository;
        _fornecedorRepository = fornecedorRepository;
        _context = context;
        _estoqueService = estoqueService;
        _logger = logger;
    }

    /// <summary>
    /// Listar Produtos
    /// </summary>
    /// <returns>Listar os Produtos</returns>
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] PaginationDto pagination)
    {
        try
        {
            var produtos = await _produtoRepository.Listar();
            return HandleResult(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos");
            return HandleError("Erro ao listar produtos");
        }
    }

    /// <summary>
    /// Obter Produto
    /// </summary>
    /// <returns>Obter o Produto pelo Id</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Obter(int id)
    {
        try
        {
            var produto = await _produtoRepository.Obter(id);
            return HandleResult(produto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter produto {ProdutoId}", id);
            return HandleError($"Erro ao obter produto {id}");
        }
    }

    /// <summary>
    /// Adicionar Produto
    /// </summary>
    /// <returns>Adicionar/Registrar um novo Produto</returns>
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] ProdutoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return HandleError("Dados inválidos", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            // Verifica se o fornecedor existe
            var fornecedor = await _fornecedorRepository.Obter(dto.IdFornecedor);
            if (fornecedor == null)
                return HandleError("Fornecedor não encontrado");

            // Validação dos veículos (se aplicável)
            if (dto.IdsVeiculosCompativeis != null && dto.IdsVeiculosCompativeis.Any())
            {
                var veiculosExistentes = await _context.Veiculos
                    .Where(v => dto.IdsVeiculosCompativeis.Contains(v.Id))
                    .CountAsync();

                if (veiculosExistentes != dto.IdsVeiculosCompativeis.Count)
                    return HandleError("Um ou mais IDs de veículos não existem");
            }

            var produto = new Produto
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = dto.Preco,
                QuantidadeEstoque = dto.QuantidadeEstoque,
                QuantidadeMinima = dto.QuantidadeMinima,
                Imagem = dto.Imagem,
                IdFornecedor = dto.IdFornecedor
            };

            await _produtoRepository.Adicionar(produto);
            return HandleResult(produto, "Produto adicionado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar produto");
            return HandleError("Erro ao adicionar produto");
        }
    }

    /// <summary>
    /// Atualizar Produto
    /// </summary>
    /// <returns>Atualizar um Produto</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] ProdutoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return HandleError("Dados inválidos", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            var produto = await _produtoRepository.Obter(id);
            if (produto == null)
                return HandleError("Produto não encontrado");

            produto.Nome = dto.Nome;
            produto.Descricao = dto.Descricao;
            produto.Preco = dto.Preco;
            produto.QuantidadeEstoque = dto.QuantidadeEstoque;
            produto.QuantidadeMinima = dto.QuantidadeMinima;
            produto.Imagem = dto.Imagem;
            produto.IdFornecedor = dto.IdFornecedor;

            await _produtoRepository.Atualizar(produto);
            return HandleResult(produto, "Produto atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar produto {ProdutoId}", id);
            return HandleError($"Erro ao atualizar produto {id}");
        }
    }

    /// <summary>
    /// Remover Produto
    /// </summary>
    /// <returns>Exluir um Produto</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            var produto = await _produtoRepository.Obter(id);
            if (produto == null)
                return HandleError("Produto não encontrado");

            await _produtoRepository.Remover(produto);
            return HandleResult(true, "Produto removido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover produto {ProdutoId}", id);
            return HandleError($"Erro ao remover produto {id}");
        }
    }

    /// <summary>
    /// Buscar Produtos por Veiculo
    /// </summary>
    /// <returns>Listar Produtos por Veiculo</returns>
    [HttpGet("veiculo/{idVeiculo}")]
    public async Task<IActionResult> BuscarPorVeiculo(int idVeiculo)
    {
        try
        {
            var produtos = await _produtoRepository.BuscarPorVeiculo(idVeiculo);
            return HandleResult(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar produtos por veículo {VeiculoId}", idVeiculo);
            return HandleError($"Erro ao buscar produtos por veículo {idVeiculo}");
        }
    }

    /// <summary>
    /// Atualizar Estoque
    /// </summary>
    /// <returns>Atualizar a quantidade do produto no Estoque</returns>
    [HttpPost("{id}/estoque")]
    public async Task<IActionResult> AtualizarEstoque(int id, [FromBody] int quantidade)
    {
        try
        {
            await _estoqueService.AtualizarEstoque(id, quantidade);
            return HandleResult(true, "Estoque atualizado com sucesso");
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, "Erro de negócio ao atualizar estoque");
            return HandleError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar estoque do produto {ProdutoId}", id);
            return HandleError($"Erro ao atualizar estoque do produto {id}");
        }
    }
}