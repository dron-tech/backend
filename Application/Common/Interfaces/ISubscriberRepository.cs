using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ISubscriberRepository : IRepository<Subscriber>
{
    public Task<bool> CheckIsUserSubscriber(int userId, int followsId);
    public Task<Subscriber?> GetSubscriberByFollowsId(int userId, int followsId);
}
