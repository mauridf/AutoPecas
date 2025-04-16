using AutoPecas.Core.Entities;
using AutoPecas.Core.Exceptions;
using AutoPecas.Core.Interfaces;
using AutoPecas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoPecas.Infrastructure.Repositories;

public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
{
    public VeiculoRepository(AutoPecasDbContext context) : base(context) { }

    public async Task<IEnumerable<Veiculo>> BuscarPorMarca(string marca)
    {
        try
        {
            return await _context.Veiculos
                .AsNoTracking()
                .Where(v => v.Marca.Contains(marca))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao buscar veículos por marca: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Veiculo>> BuscarPorNome(string nome)
    {
        try
        {
            return await _context.Veiculos
                .AsNoTracking()
                .Where(v => v.Nome.Contains(nome))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao buscar veículos por nome: {ex.Message}", ex);
        }
    }
}