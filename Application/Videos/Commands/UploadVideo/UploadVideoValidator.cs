using Application.Common;
using Application.Common.Utils;
using FluentValidation;

namespace Application.Videos.Commands.UploadVideo;

public class UploadVideoValidator : AbstractValidator<UploadVideoCmd>
{
    public UploadVideoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.Desc)
            .MaximumLength(ValidateConstant.MaxDescLength);

        RuleFor(x => x.Location)
            .MaximumLength(ValidateConstant.MaxLocationLength);
        
        RuleFor(x => x.Cover)
            .Must(x => x is { Length: <= ValidateConstant.MaxImageSize } or null)
            .WithMessage(ValidateConstant.MaxImageSizeFailMessage)
            .Must(x => (x != null && FileHelperUtil.CheckValidFileExtension(x.ContentType)) || x == null)
            .WithMessage(ValidateConstant.ImageExtensionFailMessage);

        RuleFor(x => x.Video)
            .NotEmpty()
            .Must(x => x is { Length: <= ValidateConstant.MaxVideoSize })
            .WithMessage(ValidateConstant.MaxVideoSizeFailMsg)
            .Must(x => x != null && FileHelperUtil.CheckValidVideoExtension(x.ContentType))
            .WithMessage(ValidateConstant.VideoExtensionFailMsg);
        
        RuleFor(x => x.LikeStyle)
            .IsInEnum();

        RuleFor(x => x.CommentType)
            .IsInEnum();
    }
}
