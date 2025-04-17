using AutoPecas.API.Controllers;
using AutoPecas.Core.DTOs;
using AutoPecas.Core.Entities;
using AutoPecas.Core.Exceptions;
using AutoPecas.Core.Interfaces;
using AutoPecas.Core.Services;
using AutoPecas.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class ProdutoControllerTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly Mock<IFornecedorRepository> _fornecedorRepositoryMock;
    private readonly Mock<IEstoqueService> _estoqueServiceMock;
    private readonly Mock<ILogger<ProdutoController>> _loggerMock;
    private readonly ProdutoController _controller;
    private readonly DbContextOptions<AutoPecasDbContext> _dbContextOptions;

    public ProdutoControllerTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _fornecedorRepositoryMock = new Mock<IFornecedorRepository>();
        _estoqueServiceMock = new Mock<IEstoqueService>();
        _loggerMock = new Mock<ILogger<ProdutoController>>();

        // Configuração do DbContext em memória para testes
        _dbContextOptions = new DbContextOptionsBuilder<AutoPecasDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _controller = new ProdutoController(
            _produtoRepositoryMock.Object,
            _fornecedorRepositoryMock.Object,
            new AutoPecasDbContext(_dbContextOptions),
            _estoqueServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Adicionar_QuandoFornecedorNaoExiste_DeveRetornarBadRequest()
    {
        // Arrange
        var dto = new ProdutoDto
        {
            Nome = "Pastilha de Freio",
            IdFornecedor = 1,
            QuantidadeEstoque = 10,
            QuantidadeMinima = 5
        };

        _fornecedorRepositoryMock.Setup(x => x.Obter(1))
            .ReturnsAsync((Fornecedor)null);

        // Act
        var result = await _controller.Adicionar(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Fornecedor não encontrado", badRequestResult.Value?.ToString());
    }

    [Fact]
    public async Task AtualizarEstoque_QuandoProdutoNaoExiste_DeveRetornarBadRequest()
    {
        // Arrange
        _estoqueServiceMock.Setup(x => x.AtualizarEstoque(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new BusinessException("Produto não encontrado"));

        // Act
        var result = await _controller.AtualizarEstoque(1, -3);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Produto não encontrado", badRequestResult.Value?.ToString());
    }

    [Fact]
    public async Task Obter_QuandoProdutoExiste_DeveRetornarOk()
    {
        // Arrange
        var produto = new Produto { Id = 1, Nome = "Filtro de Óleo" };
        _produtoRepositoryMock.Setup(x => x.Obter(1))
            .ReturnsAsync(produto);

        // Act
        var result = await _controller.Obter(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProduto = Assert.IsType<Produto>(okResult.Value);
        Assert.Equal(produto.Nome, returnedProduto.Nome);
    }

    [Fact]
    public async Task Obter_QuandoProdutoNaoExiste_DeveRetornarNotFound()
    {
        // Arrange
        _produtoRepositoryMock.Setup(x => x.Obter(It.IsAny<int>()))
            .ReturnsAsync((Produto)null);

        // Act
        var result = await _controller.Obter(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}