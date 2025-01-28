using Application.Common.Configs;
using Application.Common.DTO.Users;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Users.Commands.ThirdPartyLoginUser;

public class ThirdPartyLoginUserHandler : IRequestHandler<ThirdPartyLoginUserCmd, SuccessAuthDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtCfg _cfg;
    private readonly IThirdPartyAuthFactory _factory;

    public ThirdPartyLoginUserHandler(IThirdPartyAuthFactory factory, IOptions<JwtCfg> options, IUnitOfWork unitOfWork)
    {
        _factory = factory;
        _unitOfWork = unitOfWork;
        _cfg = options.Value;
    }

    public async Task<SuccessAuthDto> Handle(ThirdPartyLoginUserCmd request, CancellationToken cancellationToken)
    {
        var authService = _factory.GetInstance(request.Type);

        await authService.EnsureValidToken(request.AccessToken, request.Platform);
        var profile = await authService.GetProfileInfo(request.AccessToken);

        if (profile.Email is null)
        {
            throw new BadRequestException("Email not provide");
        }
        
        var user = await _unitOfWork.UserRepository.GetByEmail(profile.Email);

        if (user is null)
        {
            var role = await _unitOfWork.RoleRepository.GetUserRole();
            user = new User
            {
                Email = profile.Email,
                Role = role,
                IsEmailConfirm = true,
            };
            
            await _unitOfWork.UserRepository.Insert(user);
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
