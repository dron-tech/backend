using MediatR;

namespace Application.Comments.Commands.AddReply;

public class AddReplyCmd : IRequest<int>
{
    public int UserId { get; set; }
    public int CommentId { get; set; }
    public string Value { get; set; } = string.Empty;
}
