using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class BaseRepository<T> : IRepository<T> where T : BaseEntity, new()
{
    protected readonly DbSet<T> Entities;

    public BaseRepository(ContextDb context)
    {
        Entities = context.Set<T>();
    }

    public Task<T?> GetById(int id)
    {
        return Entities.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<T> GetByIdOrFail(int id)
    {
        return Entities.FirstAsync(x => x.Id == id);
    }

    public async Task Insert(T entity)
    {
        await Entities.AddAsync(entity);
    }

    public async Task InsertRange(IEnumerable<T> entities)
    {
        await Entities.AddRangeAsync(entities);
    }

    public void Remove(T entity)
    {
        Entities.Remove(entity);
    }

    public void RemoveById(int id)
    {
        Entities.Remove(new T { Id = id });
    }
}
