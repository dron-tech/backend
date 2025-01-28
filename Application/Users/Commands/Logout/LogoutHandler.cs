using Application.Common.Interfaces;
using MediatR;

namespace Application.Users.Commands.Logout;

public class LogoutHandler : IRequestHandler<LogoutCmd>
{
    private readonly IUnitOfWork _unitOfWork;

    public LogoutHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(LogoutCmd request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByIdOrFail(request.UserId);
        if (user.RefreshTokenId is not {} refreshTokenId)
        {
            return;
        }
        
        _unitOfWork.RefreshTokenRepository.RemoveById(refreshTokenId);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
