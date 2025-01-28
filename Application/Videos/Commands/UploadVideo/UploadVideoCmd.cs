using Domain.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Videos.Commands.UploadVideo;

public class UploadVideoCmd : IRequest<int>
{
    public int UserId { get; set; }
    public IFormFile? Video { get; set; }
    public IFormFile? Cover { get; set; }
    public string? Desc { get; set; }
    public string? Location { get; set; }
    public CommentType CommentType { get; set; }
    public LikeStyle LikeStyle { get; set; }
}
