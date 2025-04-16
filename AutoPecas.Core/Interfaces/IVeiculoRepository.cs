using AutoPecas.Core.Entities;

namespace AutoPecas.Core.Interfaces;

public interface IVeiculoRepository : IRepository<Veiculo>
{
    Task<IEnumerable<Veiculo>> BuscarPorMarca(string marca);
    Task<IEnumerable<Veiculo>> BuscarPorNome(string nome);
}