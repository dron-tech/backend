using System.Net;
using Application.Common.Exceptions;
using WebApp.Common;

namespace WebApp.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception err)
        {
            var message = new List<string>();
            
            if (err is BaseException httpException)
            {
                context.Response.StatusCode = httpException.StatusCode;
                message = httpException.Errors;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                message.Add("Something went wrong");
                _logger.LogError("Something went wrong: {Err}", err);
            }
            
            var response = GetFormatResponse(message.ToArray(), context.Response.StatusCode);
            await context.Response.WriteAsJsonAsync(response);
        }
    }

    private static ApiResponse<string> GetFormatResponse(string[] errors, int status)
    {
        return new ApiResponse<string>
        {
            Data = "",
            Errors = errors,
            Status = status
        };
    }
}
