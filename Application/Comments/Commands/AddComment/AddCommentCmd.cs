using MediatR;

namespace Application.Comments.Commands.AddComment;

public class AddCommentCmd : IRequest<int>
{
    public int UserId { get; set; }
    public int EntityId { get; set; }
    public bool IsVideo { get; set; }
    public string Value { get; set; } = string.Empty;
}
