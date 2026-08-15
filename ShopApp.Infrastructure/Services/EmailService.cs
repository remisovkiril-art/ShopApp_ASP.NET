using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using ShopApplication.Interfaces.Services;

namespace ShopInfrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendPasswordResetEmailAsync(
        string email,
        string resetLink)
    {
        var server =
            _configuration["Smtp:Server"]
            ?? throw new InvalidOperationException(
                "SMTP server is not configured.");

        var port =
            int.Parse(
                _configuration["Smtp:Port"]
                ?? "587");

        var senderEmail =
            _configuration["Smtp:Email"]
            ?? throw new InvalidOperationException(
                "SMTP email is not configured.");

        var password =
            _configuration["Smtp:Password"]
            ?? throw new InvalidOperationException(
                "SMTP password is not configured.");

        var message = new MimeMessage();

        message.From.Add(
            new MailboxAddress(
                "Shop API",
                senderEmail));

        message.To.Add(
            MailboxAddress.Parse(email));

        message.Subject =
            "Восстановление пароля";

        message.Body =
            new TextPart("plain")
            {
                Text =
                    $"Для восстановления пароля перейдите по ссылке:\n\n" +
                    $"{resetLink}\n\n" +
                    "Ссылка действительна 15 минут."
            };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            server,
            port,
            MailKit.Security.SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            senderEmail,
            password);

        await smtp.SendAsync(message);

        await smtp.DisconnectAsync(true);
    }
}