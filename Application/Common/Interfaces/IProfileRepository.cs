using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IProfileRepository : IRepository<Profile>
{
    public Task<Profile?> GetUserProfile(int userId);
}
