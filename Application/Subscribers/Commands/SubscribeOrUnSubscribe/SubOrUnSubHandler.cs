using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Subscribers.Commands.SubscribeOrUnSubscribe;

public class SubOrUnSubHandler : IRequestHandler<SubOrUnSubCmd>
{
    private readonly IUnitOfWork _unitOfWork;

    public SubOrUnSubHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SubOrUnSubCmd request, CancellationToken cancellationToken)
    {
        if (request.UserId == request.PublisherId)
        {
            throw new BadRequestException("You can't subscribe on myself");
        }
        
        var publisher = await _unitOfWork.UserRepository.GetById(request.PublisherId);
        if (publisher is null)
        {
            throw new NotFoundException("Publisher not found");
        }

        var existSub =
            await _unitOfWork.SubscriberRepository.GetSubscriberByFollowsId(request.UserId, request.PublisherId);

        if (existSub is null)
        {
            var subscriber = new Subscriber
            {
                UserId = request.UserId,
                FollowsId = publisher.Id
            };

            await _unitOfWork.SubscriberRepository.Insert(subscriber);
        }
        else
        {
            _unitOfWork.SubscriberRepository.Remove(existSub);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
