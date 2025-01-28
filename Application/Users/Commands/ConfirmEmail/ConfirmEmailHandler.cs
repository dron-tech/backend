using Application.Common.Configs;
using Application.Common.DTO.Users;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Users.Commands.ConfirmEmail;

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCmd, SuccessAuthDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtCfg _cfg;

    public ConfirmEmailHandler(IUnitOfWork unitOfWork, IOptions<JwtCfg> opts)
    {
        _unitOfWork = unitOfWork;
        _cfg = opts.Value;
    }

    public async Task<SuccessAuthDto> Handle(ConfirmEmailCmd request, CancellationToken cancellationToken)
    {
        var user = await GetValidUserOrFail(request.UserId);

        if (user.ConfirmCodeId is not {} codeId)
        {
            throw new BadRequestException("Incorrect code");
        }

        await EnsureCodeValid(request.Code, codeId);

        if (user.RefreshTokenId is { } refreshTokenId)
        {
            _unitOfWork.RefreshTokenRepository.RemoveById(refreshTokenId);
        }
        
        user.IsEmailConfirm = true;
        user.RefreshToken = GenRefreshToken();
        
        await _unitOfWork.CommitAsync(cancellationToken);

        var role = await _unitOfWork.RoleRepository.GetById(user.RoleId)
                ?? throw new Exception("User not contain role");
        
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

    private async Task<User> GetValidUserOrFail(int userId)
    {
        var user = await _unitOfWork.UserRepository.GetById(userId) 
                   ?? throw new NotFoundException("User not found");

        if (user.IsEmailConfirm)
        {
            throw new BadRequestException("Already confirm");
        }

        return user;
    }

    private async Task EnsureCodeValid(int reqCode, int codeId)
    {
        var code = await _unitOfWork.ConfirmCodeRepository.GetById(codeId)
                   ?? throw new BadRequestException("Incorrect code");
        
        if (code.ExpiryAt < DateTime.UtcNow)
        {
            throw new BadRequestException("Code was expired");
        }

        if (code.Code != reqCode)
        {
            throw new BadRequestException("Incorrect code");
        }
        
        _unitOfWork.ConfirmCodeRepository.Remove(code);
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
