using Application.Common.Interfaces;
using Domain.Entities;
using Persistence.Repositories;

namespace Persistence;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ContextDb _context;

    private IUserRepository? _userRepository;
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
    
    private IRoleRepository? _roleRepository;
    public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_context);

    private IRepository<ConfirmCode>? _confirmCodeRepository;
    public IRepository<ConfirmCode> ConfirmCodeRepository =>
        _confirmCodeRepository ??= new BaseRepository<ConfirmCode>(_context);
    
    private IRepository<RefreshToken>? _refreshTokenRepository;
    public IRepository<RefreshToken> RefreshTokenRepository =>
        _refreshTokenRepository ??= new BaseRepository<RefreshToken>(_context);

    private IProfileRepository? _profileRepository;
    public IProfileRepository ProfileRepository =>
        _profileRepository ??= new ProfileRepository(_context);
    
    private IVideoRepository? _videoRepository;

    public IVideoRepository VideoRepository =>
        _videoRepository ??= new VideoRepository(_context);

    private INftRepository? _nftRepository;

    public INftRepository NftRepository =>
        _nftRepository ??= new NftRepository(_context);
    
    private ILikeRepository? _likeRepository;
    public ILikeRepository LikeRepository => _likeRepository ??= new LikeRepository(_context);

    private ICommentRepository? _commentRepository;
    public ICommentRepository CommentRepository => _commentRepository ??= new CommentRepository(_context);

    private ISubscriberRepository? _subscriberRepository;
    public ISubscriberRepository SubscriberRepository => _subscriberRepository ??= new SubscriberRepository(_context);
    
    private IPublishOptionsRepository? _publishOptionsRepository;
    public IPublishOptionsRepository PublishOptionsRepository =>
        _publishOptionsRepository ??= new PublishOptionsRepository(_context);

    public UnitOfWork(ContextDb context)
    {
        _context = context;
    }

    public async Task CommitAsync(CancellationToken token)
    {
        await _context.SaveChangesAsync(token);
    }

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
