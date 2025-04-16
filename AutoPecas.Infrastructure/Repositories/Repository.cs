using AutoPecas.Core.Exceptions;
using AutoPecas.Core.Interfaces;
using AutoPecas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AutoPecas.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AutoPecasDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AutoPecasDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T> Obter(int id)
    {
        try
        {
            return await _dbSet.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao obter entidade: {ex.Message}", ex);
        }
    }

    public virtual async Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao buscar entidades: {ex.Message}", ex);
        }
    }

    public virtual async Task<IEnumerable<T>> Listar()
    {
        try
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao listar entidades: {ex.Message}", ex);
        }
    }

    public virtual async Task Adicionar(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao adicionar entidade: {ex.Message}", ex);
        }
    }

    public virtual async Task Atualizar(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao atualizar entidade: {ex.Message}", ex);
        }
    }

    public virtual async Task Remover(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao remover entidade: {ex.Message}", ex);
        }
    }
}