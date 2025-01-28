using MediatR;

namespace Application.Subscribers.Commands.SubscribeOrUnSubscribe;

public class SubOrUnSubCmd : IRequest
{
    public int UserId { get; set; }
    public int PublisherId { get; set; }
}
