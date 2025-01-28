using Application.Common.DTO;
using Application.Common.Enums;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    public Task<int> GetCommentCount(int entityId, ContentType type);
    public Task<PagedList<Comment>> GetPagedList(int entityId, ContentType type, int pgIndex,
        int pgSize);

}
