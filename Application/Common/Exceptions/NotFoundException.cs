namespace Application.Common.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message) : base(message, 404)
    {
    }

    public NotFoundException(List<string> messages) : base(messages, 404)
    {
    }
}
