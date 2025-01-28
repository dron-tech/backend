using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class ProfileRepository : BaseRepository<Profile>, IProfileRepository
{
    public ProfileRepository(ContextDb context) : base(context)
    {
    }

    public Task<Profile?> GetUserProfile(int userId)
    {
        return Entities.FirstOrDefaultAsync(x => x.User.Id == userId);
    }
}
