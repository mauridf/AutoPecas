using AutoPecas.Core.Entities;

namespace AutoPecas.Core.Interfaces;

public interface IVendedorRepository : IRepository<Vendedor>
{
    Task<Vendedor> ObterPorEmail(string email);
    Task<IEnumerable<Vendedor>> BuscarPorNome(string nome);
}