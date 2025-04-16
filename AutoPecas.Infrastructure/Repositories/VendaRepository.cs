using AutoPecas.Core.Entities;
using AutoPecas.Core.Exceptions;
using AutoPecas.Core.Interfaces;
using AutoPecas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoPecas.Infrastructure.Repositories
{
    public class VendaRepository : Repository<Venda>, IVendaRepository
    {
        public VendaRepository(AutoPecasDbContext context) : base(context) { }

        public async Task<Venda> ObterComItens(int id)
        {
            try
            {
                return await _context.Vendas
                    .Include(v => v.Itens)
                        .ThenInclude(i => i.Produto)
                    .Include(v => v.Cliente)
                    .Include(v => v.Vendedor)
                    .FirstOrDefaultAsync(v => v.Id == id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Erro ao obter venda com itens: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Venda>> ListarPorPeriodo(DateTime inicio, DateTime fim)
        {
            try
            {
                return await _context.Vendas
                    .Where(v => v.Data >= inicio && v.Data <= fim)
                    .Include(v => v.Cliente)
                    .Include(v => v.Vendedor)
                    .OrderByDescending(v => v.Data)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Erro ao listar vendas por período: {ex.Message}", ex);
            }
        }
    }
}