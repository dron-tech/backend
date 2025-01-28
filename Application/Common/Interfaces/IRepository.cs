using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    public Task Insert(T entity);
    public Task<T?> GetById(int id);
    public Task InsertRange(IEnumerable<T> entities);
    public void Remove(T entity);
    public Task<T> GetByIdOrFail(int id);
    public void RemoveById(int id);
}
