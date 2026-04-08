using Domain.Core.Aggreggates;
using Microsoft.EntityFrameworkCore;
using Repository.UnitOfWork.Abstractions;

namespace Repository.UnitOfWork;

public class UnitOfWork<T> : IUnitOfWork<T> where T : BaseDomain, new()
{
    private readonly DbContext _context;
    private IUnitOfWorkRepository<T>? _repository;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public IUnitOfWorkRepository<T> Repository
    {
        get
        {
            return _repository ??= new BaseUnitOfWork<T>(_context);
        }
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
