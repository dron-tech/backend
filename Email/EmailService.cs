using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Email.Common;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Email;

public class EmailService : IEmailService
{
    private readonly EmailCfg _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailCfg> opt, ILogger<EmailService> logger)
    {
        _config = opt.Value;
        _logger = logger;
    }

    public async Task SendConfirmCode(string toEmail, int code, string name)
    {
        try
        {
            var message = new MimeMessage();
            
            message.From.Add(new MailboxAddress("NoReply", $"{_config.User}@{_config.Domain}"));
            message.To.Add(new MailboxAddress(name, toEmail));
            message.Subject = "Itbit";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = code.ToString(),
            };
            
            message.Body = bodyBuilder.ToMessageBody();

            var client = new SmtpClient();

            await client.ConnectAsync(_config.Server, _config.Port, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(_config.User, _config.Password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch(Exception e)
        {
            _logger.LogCritical("Error near email sending {E}", e.Message);
            throw new BadRequestException("Invalid email address");
        }
    }
}
