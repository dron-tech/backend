using Application.Common;
using Application.Common.Utils;
using FluentValidation;

namespace Application.Profiles.Commands.UpdateProfile;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileCmd>
{
    public UpdateProfileValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        
        RuleFor(x => x.Cover)
            .Must(x => x is { Length: <= ValidateConstant.MaxImageSize } or null)
            .WithMessage(ValidateConstant.MaxImageSizeFailMessage)
            .Must(x => (x != null && FileHelperUtil.CheckValidFileExtension(x.ContentType)) || x == null)
            .WithMessage(ValidateConstant.ImageExtensionFailMessage);
        
        RuleFor(x => x.Avatar)
            .Must(x => x is { Length: <= ValidateConstant.MaxImageSize } or null)
            .WithMessage(ValidateConstant.MaxImageSizeFailMessage)
            .Must(x => (x != null && FileHelperUtil.CheckValidFileExtension(x.ContentType)) || x == null)
            .WithMessage(ValidateConstant.ImageExtensionFailMessage);

        RuleFor(x => x.Name)
            .MaximumLength(ValidateConstant.MaxNameLength);

        RuleFor(x => x.Desc)
            .MaximumLength(ValidateConstant.MaxDescLength);

        RuleFor(x => x.Status)
            .IsInEnum();

        RuleFor(x => x.Link)
            .MinimumLength(ValidateConstant.MinDefaultStrLength)
            .MaximumLength(ValidateConstant.MaxDefaultStrLength);
        
        RuleFor(x => x.Login)
            .MaximumLength(ValidateConstant.MaxDefaultStrLength);
    }
}

