namespace Application.Common.Exceptions;

public class BaseException : Exception
{
    public List<string> Errors { get; }
    public int StatusCode { get; protected set; }

    public BaseException(string message, int statusCode) : base(message)
    {
        Errors = new List<string>{ message };
        StatusCode = statusCode;
    }
    
    public BaseException(List<string> messages, int statusCode) : base(string.Join(", ", messages))
    {
        Errors = messages;
        StatusCode = statusCode;
    }
}
