using AutoPecas.Infrastructure.Data;
using AutoPecas.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public abstract class RepositoryTests<T> where T : class
{
    protected readonly AutoPecasDbContext _context;
    protected readonly Repository<T> _repository;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AutoPecasDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AutoPecasDbContext(options);
        _repository = new Repository<T>(_context);
    }
}