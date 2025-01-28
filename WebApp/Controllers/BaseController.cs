using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Common;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected readonly IMediator Mediator;
    protected const int CreatedHttpStatus = 201;
    
    protected BaseController(IMediator mediator)
    {
        Mediator = mediator;
    }

    protected static ApiResponse<T> GetFormattedResponse<T>(T data, int status = 200)
    {
        var result = new ApiResponse<T>
        {
            Data = data,
            Status = status
        };

        return result;
    }
    
    protected int GetUserId()
    {
        var subClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier) 
                       ?? throw new Exception("Not valid JWT: name identifier not provide");
        
        var result = int.TryParse(subClaim.Value, out var id);
        if (!result)
        {
            throw new Exception("Not valid JWT: incorrect name identifier format");
        }
    
        return id;
    }
}
