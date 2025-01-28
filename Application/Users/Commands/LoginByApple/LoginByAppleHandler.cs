using Application.Common.Configs;
using Application.Common.DTO.Users;
using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Users.Commands.LoginByApple;

public class LoginByAppleHandler : IRequestHandler<LoginByAppleCmd, SuccessAuthDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppleAuthService _appleAuthService;
    private readonly JwtCfg _cfg;

    public LoginByAppleHandler(IUnitOfWork unitOfWork, IAppleAuthService appleAuthService, IOptions<JwtCfg> opts)
    {
        _unitOfWork = unitOfWork;
        _appleAuthService = appleAuthService;
        _cfg = opts.Value;
    }

    public async Task<SuccessAuthDto> Handle(LoginByAppleCmd request, CancellationToken cancellationToken)
    {
        var email = await _appleAuthService.GetEmailByCodeOrFail(request.Code);
        var user = await _unitOfWork.UserRepository.GetByEmail(email);
        if (user is null)
        {
            throw new Exception($"User with email {email} not found");
        }

        user.RefreshToken = GenRefreshToken();
        await _unitOfWork.CommitAsync(cancellationToken);
        
        var userRole = await _unitOfWork.RoleRepository.GetByIdOrFail(user.RoleId);

        var accessTokenExpiryAt = DateTime.UtcNow.AddMinutes(_cfg.AccessTokenExpiresInMin);
        var accessToken = 
            TokenUtil.GenerateAccessToken(user.Id, userRole.Name, user.IsEmailConfirm, accessTokenExpiryAt, _cfg);

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
