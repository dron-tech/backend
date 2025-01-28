namespace Application.Common.Interfaces;

public interface IEmailService
{
    public Task SendConfirmCode(string toEmail, int code, string name);
}
