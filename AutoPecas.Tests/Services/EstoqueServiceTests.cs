using AutoPecas.Core.Entities;
using AutoPecas.Core.Interfaces;
using AutoPecas.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

public class EstoqueServiceTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly Mock<IRepository<EstoqueNotificacao>> _notificacaoRepositoryMock;
    private readonly EstoqueService _service;

    public EstoqueServiceTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _notificacaoRepositoryMock = new Mock<IRepository<EstoqueNotificacao>>();
        _service = new EstoqueService(
            _produtoRepositoryMock.Object,
            _notificacaoRepositoryMock.Object,
            Mock.Of<ILogger<EstoqueService>>());
    }

    [Fact]
    public async Task AtualizarEstoque_DeveAtualizarQuantidadeERetornarSucesso()
    {
        // Arrange
        var produto = new Produto { Id = 1, QuantidadeEstoque = 10, QuantidadeMinima = 5 };
        _produtoRepositoryMock.Setup(x => x.Obter(1)).ReturnsAsync(produto);

        // Act
        await _service.AtualizarEstoque(1, -3);

        // Assert
        Assert.Equal(7, produto.QuantidadeEstoque);
        _produtoRepositoryMock.Verify(x => x.Atualizar(produto), Times.Once);
    }

    [Fact]
    public async Task AtualizarEstoque_QuandoEstoqueFicarAbaixoDoMinimo_DeveGerarNotificacao()
    {
        // Arrange
        var produto = new Produto { Id = 1, QuantidadeEstoque = 6, QuantidadeMinima = 5 };
        _produtoRepositoryMock.Setup(x => x.Obter(1)).ReturnsAsync(produto);
        _notificacaoRepositoryMock.Setup(x => x.Buscar(It.IsAny<Expression<Func<EstoqueNotificacao, bool>>>()))
            .ReturnsAsync(new List<EstoqueNotificacao>());

        // Act
        await _service.AtualizarEstoque(1, -2);

        // Assert
        _notificacaoRepositoryMock.Verify(x => x.Adicionar(It.Is<EstoqueNotificacao>(n =>
            n.IdProduto == 1 && n.QuantidadeAtual == 4 && !n.Resolvido)), Times.Once);
    }
}