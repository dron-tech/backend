using Application.Common.Configs;
using Application.Common.DTO.Users;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Users.Commands.RefreshUserToken;

public class RefreshUserTokenHandler : IRequestHandler<RefreshUserTokenCmd, SuccessAuthDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtCfg _cfg;

    public RefreshUserTokenHandler(IUnitOfWork unitOfWork, IOptions<JwtCfg> opts)
    {
        _unitOfWork = unitOfWork;
        _cfg = opts.Value;
    }

    public async Task<SuccessAuthDto> Handle(RefreshUserTokenCmd request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetById(request.UserId);
        if (user?.RefreshTokenId is not {} refreshTokenId)
        {
            throw new NotFoundException("User not found");
        }

        var token = await _unitOfWork.RefreshTokenRepository.GetByIdOrFail(refreshTokenId);
        if (token.Token != request.RefreshToken || token.ExpiryAt < DateTime.UtcNow)
        {
            throw new BadRequestException("Incorrect token");
        }

        var role = await _unitOfWork.RoleRepository.GetByIdOrFail(user.RoleId);
        _unitOfWork.RefreshTokenRepository.Remove(token);
        
        user.RefreshToken = GenRefreshToken();
        await _unitOfWork.CommitAsync(cancellationToken);
        
        var accessTokenExpiryAt = DateTime.UtcNow.AddMinutes(_cfg.AccessTokenExpiresInMin);
        var accessToken = 
            TokenUtil.GenerateAccessToken(user.Id, role.Name, user.IsEmailConfirm, accessTokenExpiryAt, _cfg);
        
        return new SuccessAuthDto
        {
            AccessToken = accessToken,
            RefreshToken = user.RefreshToken.Token,
            AccessExpiryAt = accessTokenExpiryAt,
            RefreshExpiryAt = user.RefreshToken.ExpiryAt
        };
    }
    
    private RefreshToken GenRefreshToken()
    {
        return new RefreshToken
        {
            Token = TokenUtil.GenerateRefreshToken(),
            ExpiryAt = DateTime.UtcNow.AddDays(_cfg.RefreshTokenExpiresInDays),
        };
    }
}
