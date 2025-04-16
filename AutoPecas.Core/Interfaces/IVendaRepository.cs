using AutoPecas.Core.Entities;

namespace AutoPecas.Core.Interfaces;

public interface IVendaRepository : IRepository<Venda>
{
    Task<Venda> ObterComItens(int id);
    Task<IEnumerable<Venda>> ListarPorPeriodo(DateTime inicio, DateTime fim);
}