using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ContextDb context) : base(context)
    {
    }

    public Task<bool> ExistByEmail(string email)
    {
        return Entities.AnyAsync(x => x.Email == email);
    }

    public Task<bool> ExistByLogin(string login)
    {
        return Entities.AnyAsync(x => x.Login == login);
    }

    public Task<User?> GetByEmailOrLogin(string str)
    {
        return Entities.FirstOrDefaultAsync(x => x.Login == str || x.Email == str);
    }

    public Task<User?> GetByEmail(string email)
    {
        return Entities.FirstOrDefaultAsync(x => x.Email == email);
    }

    public Task<User?> GetByAppleSub(string sub)
    {
        return Entities.FirstOrDefaultAsync(x => x.AppleSub == sub);
    }

    public Task<string> GetUserLoginOrFail(int id)
    {
        return Entities
            .Where(x => x.Id == id)
            .Select(x => x.Login)
            .FirstAsync();
    }
}
