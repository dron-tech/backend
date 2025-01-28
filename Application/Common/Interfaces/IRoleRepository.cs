using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IRoleRepository : IRepository<Role>
{
    public Task<Role> GetUserRole();
    public Task<Role> GetAdminRole();

}
