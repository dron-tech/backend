using FluentValidation;

namespace Application.Nfts.Commands.MapNftUrlList;

public class MapNftUrlListValidator : AbstractValidator<MapNftUrlListCmd>
{
    public MapNftUrlListValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.Urls)
            .NotEmpty();
    }
}
