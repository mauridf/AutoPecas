using AutoPecas.Core.Entities;

namespace AutoPecas.Core.Interfaces;

public interface IFornecedorRepository : IRepository<Fornecedor>
{
    Task<IEnumerable<Fornecedor>> BuscarPorNome(string nome);
    Task<IEnumerable<Fornecedor>> BuscarPorProduto(int idProduto);
}