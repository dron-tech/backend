using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(ContextDb context) : base(context)
    {
    }

    public Task<Role> GetUserRole()
    {
        return Entities.FirstAsync(x => x.Name == UserRoles.User);
    }

    public Task<Role> GetAdminRole()
    {
        return Entities.FirstAsync(x => x.Name == UserRoles.Admin);
    }
}
