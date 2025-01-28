using Application.Common.DTO;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IVideoRepository : IRepository<Video>
{
    public Task<Video?> GetUserVideoById(int videoId, int userId);
    public Task<PagedList<Video>> GetUserVideoList(int userId, int pgIndex, int pgSize);
    public Task<bool> IsVideoExist(int videoId);
    public Task<int> GetUserVideoCount(int userId);
}
