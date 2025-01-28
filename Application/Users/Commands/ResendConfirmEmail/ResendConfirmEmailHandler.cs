using Application.Common.Configs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Users.Commands.ResendConfirmEmail;

public class ResendConfirmEmailHandler : IRequestHandler<ResendConfirmEmailCmd>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ConfirmCodeCfg _cfg;

    public ResendConfirmEmailHandler(IUnitOfWork unitOfWork, IOptions<ConfirmCodeCfg> opts, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _cfg = opts.Value;
    }

    public async Task Handle(ResendConfirmEmailCmd request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByIdOrFail(request.UserId);
        if (user.IsEmailConfirm)
        {
            throw new BadRequestException("Email already confirm");
        }

        if (user.ConfirmCodeId is not { } codeId)
        {
            await GenNewCodeAndSend(user, cancellationToken);
            return;
        }

        var code = await _unitOfWork.ConfirmCodeRepository.GetByIdOrFail(codeId);
        if (code.ExpiryAt > DateTime.UtcNow)
        {
            throw new BadRequestException(
                $"You will be able to request the code again no later than {code.ExpiryAt}");
        }
        
        _unitOfWork.ConfirmCodeRepository.Remove(code);
        await GenNewCodeAndSend(user, cancellationToken);
    }

    private async Task GenNewCodeAndSend(User user, CancellationToken token)
    {
        user.ConfirmCode = GenConfirmEmailCode();
        await _unitOfWork.CommitAsync(token);
            
        await _emailService.SendConfirmCode(user.Email, user.ConfirmCode.Code, user.Login);
    }
    
    private ConfirmCode GenConfirmEmailCode()
    {
        return new ConfirmCode
        {
            Code = TokenUtil.GenerateCode(),
            ExpiryAt = DateTime.UtcNow.AddMinutes(_cfg.LifeTimeInMin),
        };
    }
}
