using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class SubscriberRepository : BaseRepository<Subscriber>, ISubscriberRepository
{
    public SubscriberRepository(ContextDb context) : base(context)
    {
    }

    public Task<bool> CheckIsUserSubscriber(int userId, int followsId)
    {
        return Entities.AnyAsync(x => x.UserId == userId && x.FollowsId == followsId);
    }

    public Task<Subscriber?> GetSubscriberByFollowsId(int userId, int followsId)
    {
        return Entities.FirstOrDefaultAsync(x => x.UserId == userId && x.FollowsId == followsId);
    }
}
