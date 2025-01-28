using Application.Common.Configs;
using Application.Common.DTO.Users;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Users.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCmd, SuccessAuthDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtCfg _cfg;

    public LoginHandler(IUnitOfWork unitOfWork, IOptions<JwtCfg> opts)
    {
        _unitOfWork = unitOfWork;
        _cfg = opts.Value;
    }

    public async Task<SuccessAuthDto> Handle(LoginCmd request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailOrLogin(request.EmailOrLogin);
        if (user is null || !HasherUtil.VerifyPassword(request.Psw, user.HashedPsw))
        {
            throw new BadRequestException("Incorrect login or password");
        }

        var role = await _unitOfWork.RoleRepository.GetByIdOrFail(user.RoleId);
        
        if (user.RefreshTokenId is { } refreshTokenId)
        {
            _unitOfWork.RefreshTokenRepository.RemoveById(refreshTokenId);
        }
        
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
