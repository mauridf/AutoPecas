using AutoPecas.Core.Entities;

namespace AutoPecas.Core.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> BuscarPorVeiculo(int idVeiculo);
    Task<IEnumerable<Produto>> BuscarComBaixoEstoque();
}