using AutoPecas.Core.Entities;

namespace AutoPecas.Core.Interfaces;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<Cliente> ObterPorDocumento(string documento);
    Task<IEnumerable<Cliente>> BuscarPorNome(string nome);
}