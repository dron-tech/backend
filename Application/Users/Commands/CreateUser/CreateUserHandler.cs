using Application.Common.Configs;
using Application.Common.DTO.Users;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Users.Commands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, SuccessAuthDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtCfg _jwtCfg;
    private readonly ConfirmCodeCfg _codeCfg;
    private readonly IEmailService _emailService;

    public CreateUserHandler(IOptions<JwtCfg> jwtOpts, IOptions<ConfirmCodeCfg> codeOpts, IEmailService emailService,
        IUnitOfWork unitOfWork)
    {
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _jwtCfg = jwtOpts.Value;
        _codeCfg = codeOpts.Value;
    }

    public async Task<SuccessAuthDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await EnsureUniqueCredentials(request.Login, request.Email);
        var role = await _unitOfWork.RoleRepository.GetUserRole();

        var user = new User
        {
            Login = request.Login,
            HashedPsw = HasherUtil.GetHashedPassword(request.Psw),
            Email = request.Email,
            Role = role,
            ConfirmCode = GenConfirmEmailCode(),
            RefreshToken = GenRefreshToken()
        };

        await _unitOfWork.UserRepository.Insert(user);
        await _unitOfWork.CommitAsync(cancellationToken);
        
        var accessTokenExpiryAt = DateTime.UtcNow.AddMinutes(_jwtCfg.AccessTokenExpiresInMin);
        var accessToken = 
            TokenUtil.GenerateAccessToken(user.Id, role.Name, user.IsEmailConfirm, accessTokenExpiryAt, _jwtCfg);

        await _emailService.SendConfirmCode(user.Email, user.ConfirmCode.Code, user.Login);

        return new SuccessAuthDto
        {
            AccessToken = accessToken,
            RefreshToken = user.RefreshToken.Token,
            AccessExpiryAt = accessTokenExpiryAt,
            RefreshExpiryAt = user.RefreshToken.ExpiryAt
        };
    }

    private async Task EnsureUniqueCredentials(string login, string email)
    {
        if (await _unitOfWork.UserRepository.ExistByEmail(email))
        {
            throw new BadRequestException("Email already used");
        }

        if (await _unitOfWork.UserRepository.ExistByLogin(login))
        {
            throw new BadRequestException("Login already used");
        }
    }

    private ConfirmCode GenConfirmEmailCode()
    {
        return new ConfirmCode
        {
            Code = TokenUtil.GenerateCode(),
            ExpiryAt = DateTime.UtcNow.AddMinutes(_codeCfg.LifeTimeInMin),
        };
    }

    private RefreshToken GenRefreshToken()
    {
        return new RefreshToken
        {
            Token = TokenUtil.GenerateRefreshToken(),
            ExpiryAt = DateTime.UtcNow.AddDays(_jwtCfg.RefreshTokenExpiresInDays),
        };
    }
}
