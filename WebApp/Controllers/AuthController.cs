using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Configs;
using Application.Common.DTO.Users;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Users.Commands.ConfirmEmail;
using Application.Users.Commands.CreateUser;
using Application.Users.Commands.Login;
using Application.Users.Commands.LoginByApple;
using Application.Users.Commands.Logout;
using Application.Users.Commands.RefreshUserToken;
using Application.Users.Commands.ResendConfirmEmail;
using Application.Users.Commands.ResetPsw;
using Application.Users.Commands.SendResetPswCode;
using Application.Users.Commands.ThirdPartyLoginUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApp.Common;
using WebApp.Common.DTO.Auth;

namespace WebApp.Controllers;

public class AuthController : BaseController
{
    private readonly JwtCfg _cfg;
    
    public AuthController(IMediator mediator, IOptions<JwtCfg> opts) : base(mediator)
    {
        _cfg = opts.Value;
    }

    [HttpPost("Signup")]
    public async Task<ApiResponse<SuccessAuthDto>> Signup(CreateUserCommand cmd)
    {
        return GetFormattedResponse(await Mediator.Send(cmd));
    }
    
    [Authorize]
    [HttpPost("Email/Confirm")]
    public async Task<ApiResponse<SuccessAuthDto>> ConfirmEmail(ConfirmEmailDto dto)
    {
        var cmd = new ConfirmEmailCmd
        {
            Code = dto.Code,
            UserId = GetUserId()
        };

        return GetFormattedResponse(await Mediator.Send(cmd));
    }

    [Authorize]
    [HttpPost("Email/Resend")]
    public async Task ResendConfirmEmailCode()
    {
        var cmd = new ResendConfirmEmailCmd
        {
            UserId = GetUserId()
        };
        
        await Mediator.Send(cmd);
    }

    [HttpPost("Signin")]
    public async Task<ApiResponse<SuccessAuthDto>> Login(LoginCmd cmd)
    {
        return GetFormattedResponse(await Mediator.Send(cmd));
    }

    [Authorize]
    [HttpPost("Logout")]
    public async Task Logout()
    {
        var cmd = new LogoutCmd
        {
            UserId = GetUserId()
        };
        
        await Mediator.Send(cmd);
    }

    [HttpPost("Refresh")]
    public async Task<ApiResponse<SuccessAuthDto>> Refresh(RefreshTokenDto dto)
    {
        var nameIdent = GetNameIdentifierFromExpiredToken(dto.ExpiredAccessToken);
        if (nameIdent == null)
        {
            throw new BadHttpRequestException("Invalid access token or refresh token");
        }
        
        var result = int.TryParse(nameIdent, out var id);
        if (!result)
        {
            throw new Exception("Not valid JWT: incorrect name identifier format");
        }

        var cmd = new RefreshUserTokenCmd
        {
            UserId = id,
            RefreshToken = dto.RefreshToken
        };
        
        return GetFormattedResponse(await Mediator.Send(cmd));

    }

    [HttpPost("Password/Code")]
    public async Task SendResetPswCode(SendResetPswCodeCmd cmd)
    {
        await Mediator.Send(cmd);
    }

    [HttpPost("Password/Reset")]
    public async Task ResetPsw(ResetPswCmd cmd)
    {
        await Mediator.Send(cmd);
    }
    
    [HttpPost("Google/Android")]
    public async Task<ApiResponse<SuccessAuthDto>> GoogleAuth(ThirdPartyLoginDto dto)
    {
        return await ThirdPartyAuth(dto.AccessToken, ThirdPartyServiceType.Google, ThirdPartyPlatform.Android);
    }

    [HttpPost("Google/Ios")]
    public async Task<ApiResponse<SuccessAuthDto>> GoogleAuthIos(ThirdPartyLoginDto dto)
    {
        return await ThirdPartyAuth(dto.AccessToken, ThirdPartyServiceType.Google, ThirdPartyPlatform.Ios);
    }
    
    [HttpPost("Facebook")]
    public async Task<ApiResponse<SuccessAuthDto>> FacebookAuth(ThirdPartyLoginDto dto)
    {
        return await ThirdPartyAuth(dto.AccessToken, ThirdPartyServiceType.Facebook);
    }

    [HttpPost("Apple")]
    public async Task<ApiResponse<SuccessAuthDto>> AppleAuth(LoginByAppleCmd cmd)
    {
        return GetFormattedResponse(await Mediator.Send(cmd));
    }

    private async Task<ApiResponse<SuccessAuthDto>> ThirdPartyAuth(string accessToken, ThirdPartyServiceType type,
        ThirdPartyPlatform? platform = null)
    {
        var command = new ThirdPartyLoginUserCmd
        {
            AccessToken = accessToken,
            Type = type,
            Platform = platform
        };
        
        return GetFormattedResponse(await Mediator.Send(command));
    }

    private string? GetNameIdentifierFromExpiredToken(string? token)
    {
        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = _cfg.Audience,
                ValidIssuer = _cfg.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg.Secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
        catch (Exception)
        {
            throw new BadRequestException("Incorrect expired access token");
        }
    }
}
