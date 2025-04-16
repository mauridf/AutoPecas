using AutoPecas.Core.Entities;
using AutoPecas.Core.Exceptions;
using AutoPecas.Core.Interfaces;
using AutoPecas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoPecas.Infrastructure.Repositories;

public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
{
    public FornecedorRepository(AutoPecasDbContext context) : base(context) { }

    public async Task<IEnumerable<Fornecedor>> BuscarPorNome(string nome)
    {
        try
        {
            return await _context.Fornecedores
                .AsNoTracking()
                .Where(f => f.Nome.Contains(nome))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao buscar fornecedores por nome: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Fornecedor>> BuscarPorProduto(int idProduto)
    {
        try
        {
            return await _context.Fornecedores
                .AsNoTracking()
                .Where(f => f.Produtos.Any(p => p.Id == idProduto))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao buscar fornecedores por produto: {ex.Message}", ex);
        }
    }
}