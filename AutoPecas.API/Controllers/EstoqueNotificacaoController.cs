using AutoPecas.Core.DTOs;
using AutoPecas.Core.Interfaces;
using AutoPecas.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoPecas.Core.Exceptions;

namespace AutoPecas.API.Controllers;

/// <summary>
/// Controller para gerenciamento de estoque
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EstoqueNotificacaoController : BaseController
{
    private readonly IRepository<EstoqueNotificacao> _notificacaoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<EstoqueNotificacaoController> _logger;

    public EstoqueNotificacaoController(
        IRepository<EstoqueNotificacao> notificacaoRepository,
        IProdutoRepository produtoRepository,
        ILogger<EstoqueNotificacaoController> logger)
    {
        _notificacaoRepository = notificacaoRepository;
        _produtoRepository = produtoRepository;
        _logger = logger;
    }

    /// <summary>
    /// Listar Notificacoes
    /// </summary>
    /// <returns>Listar Notificações do Estoque</returns>
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] bool? resolvido = null)
    {
        try
        {
            IEnumerable<EstoqueNotificacao> notificacoes;

            if (resolvido.HasValue)
            {
                notificacoes = await _notificacaoRepository.Buscar(n => n.Resolvido == resolvido.Value);
            }
            else
            {
                notificacoes = await _notificacaoRepository.Listar();
            }

            var produtos = await _produtoRepository.Listar();

            var resultado = notificacoes.Select(n => new EstoqueNotificacaoRespostaDto
            {
                Id = n.Id,
                IdProduto = n.IdProduto,
                ProdutoNome = produtos.FirstOrDefault(p => p.Id == n.IdProduto)?.Nome ?? "Desconhecido",
                QuantidadeAtual = n.QuantidadeAtual,
                QuantidadeMinima = produtos.FirstOrDefault(p => p.Id == n.IdProduto)?.QuantidadeMinima ?? 0,
                DataNotificacao = n.DataNotificacao,
                Resolvido = n.Resolvido,
                DataResolucao = n.DataResolucao
            });

            return HandleResult(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar notificações de estoque");
            return HandleError("Erro ao listar notificações de estoque");
        }
    }

    /// <summary>
    /// Marcar Como Resolvido
    /// </summary>
    /// <returns>Marcar Notificação como Resolvida</returns>
    [HttpPatch("{id}/resolver")]
    public async Task<IActionResult> MarcarComoResolvido(int id)
    {
        try
        {
            var notificacao = await _notificacaoRepository.Obter(id);
            if (notificacao == null)
                return HandleError("Notificação não encontrada");

            notificacao.Resolvido = true;
            notificacao.DataResolucao = DateTime.Now;

            await _notificacaoRepository.Atualizar(notificacao);
            return HandleResult(true, "Notificação marcada como resolvida");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao marcar notificação {NotificacaoId} como resolvida", id);
            return HandleError($"Erro ao marcar notificação {id} como resolvida");
        }
    }
}
