using AutoPecas.Core.Entities;
using AutoPecas.Core.Exceptions;
using AutoPecas.Core.Interfaces;
using AutoPecas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoPecas.Infrastructure.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AutoPecasDbContext context) : base(context) { }

    public async Task<IEnumerable<Produto>> BuscarPorVeiculo(int idVeiculo)
    {
        try
        {
            return await _context.Produtos
                .AsNoTracking()
                .Where(p => p.VeiculosCompativeis.Any(v => v.Id == idVeiculo))
                .Include(p => p.Fornecedor)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao buscar produtos por veículo: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Produto>> BuscarComBaixoEstoque()
    {
        try
        {
            return await _context.Produtos
                .AsNoTracking()
                .Where(p => p.QuantidadeEstoque <= p.QuantidadeMinima)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao buscar produtos com estoque baixo: {ex.Message}", ex);
        }
    }
}