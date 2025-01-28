using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IUserRepository : IRepository<User>
{
    public Task<bool> ExistByEmail(string email);
    public Task<bool> ExistByLogin(string login);
    public Task<User?> GetByEmailOrLogin(string str);
    public Task<User?> GetByEmail(string email);
    public Task<User?> GetByAppleSub(string sub);
    public Task<string> GetUserLoginOrFail(int id);
}
