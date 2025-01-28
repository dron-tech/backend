using MediatR;

namespace Application.Videos.Commands.DeleteVideo;

public class DeleteVideoCmd : IRequest
{
    public int UserId { get; set; }
    public int VideoId { get; set; }
}
