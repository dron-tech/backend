using Application.Common;
using FluentValidation;

namespace Application.Nfts.Queries.GetTokensUri;

public class GetTokensUriValidator : AbstractValidator<GetTokensUriQuery>
{
    public GetTokensUriValidator()
    {
        RuleFor(x => x.Links)
            .NotEmpty()
            .Must(x => x is { Length: <= ValidateConstant.MaxTokensUriLinks })
            .WithMessage(ValidateConstant.MaxTokensUriLinksFailMsg);
    }
}
