using AutoPecas.Core.Entities;
using AutoPecas.Core.Interfaces;
using AutoPecas.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace AutoPecas.Core.Services;

public class EstoqueService : IEstoqueService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IRepository<EstoqueNotificacao> _notificacaoRepository;
    private readonly ILogger<EstoqueService> _logger;

    public EstoqueService(
        IProdutoRepository produtoRepository,
        IRepository<EstoqueNotificacao> notificacaoRepository,
        ILogger<EstoqueService> logger)
    {
        _produtoRepository = produtoRepository;
        _notificacaoRepository = notificacaoRepository;
        _logger = logger;
    }

    public async Task AtualizarEstoque(int idProduto, int quantidade)
    {
        try
        {
            var produto = await _produtoRepository.Obter(idProduto);
            if (produto == null)
                throw new BusinessException("Produto não encontrado");

            if (produto.QuantidadeEstoque + quantidade < 0)
                throw new BusinessException("Quantidade em estoque não pode ser negativa");

            produto.QuantidadeEstoque += quantidade;
            await _produtoRepository.Atualizar(produto);

            await VerificarEstoqueBaixo(produto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar estoque do produto {ProdutoId}", idProduto);
            throw;
        }
    }

    public async Task VerificarEstoqueBaixo()
    {
        try
        {
            var produtosComEstoqueBaixo = await _produtoRepository.BuscarComBaixoEstoque();
            foreach (var produto in produtosComEstoqueBaixo)
            {
                await GerarNotificacaoEstoqueBaixo(produto);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar estoque baixo");
            throw;
        }
    }

    private async Task VerificarEstoqueBaixo(Produto produto)
    {
        if (produto.QuantidadeEstoque <= produto.QuantidadeMinima)
        {
            await GerarNotificacaoEstoqueBaixo(produto);
        }
        else
        {
            // Se o estoque foi reposto, marcar notificações como resolvidas
            var notificacoesAbertas = await _notificacaoRepository.Buscar(
                n => n.IdProduto == produto.Id && !n.Resolvido);

            foreach (var notificacao in notificacoesAbertas)
            {
                notificacao.Resolvido = true;
                notificacao.DataResolucao = DateTime.Now;
                await _notificacaoRepository.Atualizar(notificacao);
            }
        }
    }

    private async Task GerarNotificacaoEstoqueBaixo(Produto produto)
    {
        try
        {
            var notificacaoExistente = (await _notificacaoRepository.Buscar(
                n => n.IdProduto == produto.Id && !n.Resolvido))
                .FirstOrDefault();

            if (notificacaoExistente == null)
            {
                var notificacao = new EstoqueNotificacao
                {
                    IdProduto = produto.Id,
                    QuantidadeAtual = produto.QuantidadeEstoque,
                    DataNotificacao = DateTime.Now,
                    Resolvido = false
                };
                await _notificacaoRepository.Adicionar(notificacao);

                _logger.LogWarning("Notificação de estoque baixo criada para o produto {ProdutoId} ({ProdutoNome}). Quantidade: {Quantidade}, Mínimo: {QuantidadeMinima}",
                    produto.Id, produto.Nome, produto.QuantidadeEstoque, produto.QuantidadeMinima);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar notificação de estoque baixo para o produto {ProdutoId}", produto.Id);
            throw;
        }
    }
}