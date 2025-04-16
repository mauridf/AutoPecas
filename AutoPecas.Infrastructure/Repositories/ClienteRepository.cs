using AutoPecas.Core.Entities;
using AutoPecas.Core.Exceptions;
using AutoPecas.Core.Interfaces;
using AutoPecas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoPecas.Infrastructure.Repositories;

public class ClienteRepository : Repository<Cliente>, IClienteRepository
{
    public ClienteRepository(AutoPecasDbContext context) : base(context) { }

    public async Task<Cliente> ObterPorDocumento(string documento)
    {
        try
        {
            return await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Documento == documento);
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao obter cliente por documento: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Cliente>> BuscarPorNome(string nome)
    {
        try
        {
            return await _context.Clientes
                .AsNoTracking()
                .Where(c => c.Nome.Contains(nome))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao buscar clientes por nome: {ex.Message}", ex);
        }
    }
}