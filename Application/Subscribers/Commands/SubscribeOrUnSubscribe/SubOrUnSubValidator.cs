using FluentValidation;

namespace Application.Subscribers.Commands.SubscribeOrUnSubscribe;

public class SubOrUnSubValidator : AbstractValidator<SubOrUnSubCmd>
{
    public SubOrUnSubValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.PublisherId)
            .GreaterThan(0);
    }
}
