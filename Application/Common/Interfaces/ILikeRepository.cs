using Application.Common.Enums;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ILikeRepository : IRepository<Like>
{
    public Task<Like?> GetUserLike(int userId, int entityId, ContentType type);
    public Task<int> GetLikeCount(int entityId, ContentType type);
    public Task<bool> CheckExist(int userId, int entityId, ContentType type);
}
