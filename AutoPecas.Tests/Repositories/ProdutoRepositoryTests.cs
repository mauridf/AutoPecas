using AutoPecas.Core.Entities;
using AutoPecas.Infrastructure.Data;
using AutoPecas.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class ProdutoRepositoryTests : IClassFixture<RepositoryTestsFixture>
{
    private readonly ProdutoRepository _repository;
    private readonly RepositoryTestsFixture _fixture;

    public ProdutoRepositoryTests(RepositoryTestsFixture fixture)
    {
        _fixture = fixture;
        _repository = new ProdutoRepository(_fixture.Context);
    }

    [Fact]
    public async Task BuscarPorVeiculo_DeveRetornarProdutosCompativeis()
    {
        // Arrange
        var veiculo = new Veiculo { Nome = "Onix", Marca = "Chevrolet" };
        await _fixture.Context.Veiculos.AddAsync(veiculo);

        var produto = new Produto { Nome = "Pastilha de Freio" };
        produto.VeiculosCompativeis.Add(veiculo);
        await _fixture.Context.Produtos.AddAsync(produto);
        await _fixture.Context.SaveChangesAsync();

        // Act
        var result = await _repository.BuscarPorVeiculo(veiculo.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(produto.Nome, result.First().Nome);
    }

    [Fact]
    public async Task BuscarComBaixoEstoque_DeveRetornarApenasProdutosComEstoqueBaixo()
    {
        // Arrange
        var produtos = new List<Produto>
        {
            new() { Nome = "Produto 1", QuantidadeEstoque = 5, QuantidadeMinima = 10 },
            new() { Nome = "Produto 2", QuantidadeEstoque = 15, QuantidadeMinima = 10 }
        };

        await _fixture.Context.Produtos.AddRangeAsync(produtos);
        await _fixture.Context.SaveChangesAsync();

        // Act
        var result = await _repository.BuscarComBaixoEstoque();

        // Assert
        Assert.Single(result);
        Assert.Equal("Produto 1", result.First().Nome);
    }
}

public class RepositoryTestsFixture : IDisposable
{
    public AutoPecasDbContext Context { get; }

    public RepositoryTestsFixture()
    {
        var options = new DbContextOptionsBuilder<AutoPecasDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new AutoPecasDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}