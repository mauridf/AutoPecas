using System.Linq.Expressions;

namespace AutoPecas.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> Obter(int id);
    Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> Listar();
    Task Adicionar(T entity);
    Task Atualizar(T entity);
    Task Remover(T entity);
}