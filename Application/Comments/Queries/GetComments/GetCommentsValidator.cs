using FluentValidation;

namespace Application.Comments.Queries.GetComments;

public class GetCommentsValidator : AbstractValidator<GetCommentsQuery>
{
    public GetCommentsValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        
        RuleFor(x => x.PageIndex)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .GreaterThan(0);

        RuleFor(x => x.Type)
            .IsInEnum();

        RuleFor(x => x.EntityId)
            .GreaterThan(0);
    }
}
