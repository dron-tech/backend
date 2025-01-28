using FluentValidation;

namespace Application.Videos.Commands.DeleteVideo;

public class DeleteVideoValidator : AbstractValidator<DeleteVideoCmd>
{
    public DeleteVideoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.VideoId)
            .GreaterThan(0);
    }
}
