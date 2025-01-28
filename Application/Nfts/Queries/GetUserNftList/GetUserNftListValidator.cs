using FluentValidation;

namespace Application.Nfts.Queries.GetUserNftList;

public class GetUserNftListValidator : AbstractValidator<GetUserNftListQuery>
{
    public GetUserNftListValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        
        RuleFor(x => x.PageIndex)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .GreaterThan(0);
    }
}
