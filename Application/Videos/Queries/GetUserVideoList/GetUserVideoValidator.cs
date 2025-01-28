using FluentValidation;

namespace Application.Videos.Queries.GetUserVideoList;

public class GetUserVideoValidator : AbstractValidator<GetUserVideoListQuery>
{
    public GetUserVideoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        
        RuleFor(x => x.PageIndex)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .GreaterThan(0);
    }
}
