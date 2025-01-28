using Application.Common.Configs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Users.Commands.SendResetPswCode;

public class SendResetPswCodeHandler : IRequestHandler<SendResetPswCodeCmd>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ConfirmCodeCfg _cfg;

    public SendResetPswCodeHandler(IUnitOfWork unitOfWork, IEmailService emailService, IOptions<ConfirmCodeCfg> opts)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _cfg = opts.Value;
    }

    public async Task Handle(SendResetPswCodeCmd request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailOrLogin(request.EmailOrLogin);
        if (user is null || !user.IsEmailConfirm)
        {
            throw new NotFoundException("User with provided data not found or email not confirm");
        }

        if (user.ConfirmCodeId is { } codeId)
        {
            var code = await _unitOfWork.ConfirmCodeRepository.GetByIdOrFail(codeId);
            if (code.ExpiryAt > DateTime.UtcNow)
            {
                throw new BadRequestException(
                    $"You will be able to request the code again no later than {code.ExpiryAt}");
            }
        
            _unitOfWork.ConfirmCodeRepository.Remove(code);
        }

        user.ConfirmCode = GenConfirmEmailCode();
        await _unitOfWork.CommitAsync(cancellationToken);
        
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
