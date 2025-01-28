using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Utils;
using MediatR;

namespace Application.Users.Commands.ResetPsw;

public class ResetPswHandler : IRequestHandler<ResetPswCmd>
{
    private readonly IUnitOfWork _unitOfWork;

    public ResetPswHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ResetPswCmd request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailOrLogin(request.EmailOrLogin);
        if (user is null || !user.IsEmailConfirm)
        {
            throw new NotFoundException("User with provided data not found or email not confirm");
        }

        if (user.ConfirmCodeId is not { } codeId)
        {
            throw new BadRequestException("Incorrect code");
        }

        var code = await _unitOfWork.ConfirmCodeRepository.GetByIdOrFail(codeId);
        if (request.Code != code.Code)
        {
            throw new BadRequestException("Incorrect code");
        }
        
        _unitOfWork.ConfirmCodeRepository.Remove(code);

        user.HashedPsw = HasherUtil.GetHashedPassword(request.Psw);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
