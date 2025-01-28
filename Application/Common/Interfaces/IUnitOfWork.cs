using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IUnitOfWork
{
    public Task CommitAsync(CancellationToken token);
    public IRoleRepository RoleRepository { get; }
    public IUserRepository UserRepository { get; }
    public IRepository<RefreshToken> RefreshTokenRepository { get; }
    public IRepository<ConfirmCode> ConfirmCodeRepository { get; }
    public IProfileRepository ProfileRepository { get; }
    public IVideoRepository VideoRepository { get; }
    public INftRepository NftRepository { get; }
    public ILikeRepository LikeRepository { get; }
    public ICommentRepository CommentRepository { get; }
    public IPublishOptionsRepository PublishOptionsRepository { get; }
    public ISubscriberRepository SubscriberRepository { get; }
}
