using AutoPecas.Core.Entities;
using AutoPecas.Core.Exceptions;
using AutoPecas.Core.Interfaces;
using AutoPecas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoPecas.Infrastructure.Repositories;

public class VendedorRepository : Repository<Vendedor>, IVendedorRepository
{
    public VendedorRepository(AutoPecasDbContext context) : base(context) { }

    public async Task<Vendedor> ObterPorEmail(string email)
    {
        try
        {
            return await _context.Vendedores
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Email == email);
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao obter vendedor por email: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Vendedor>> BuscarPorNome(string nome)
    {
        try
        {
            return await _context.Vendedores
                .AsNoTracking()
                .Where(v => v.Nome.Contains(nome))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao buscar vendedores por nome: {ex.Message}", ex);
        }
    }
}