namespace ShopApplication.Interfaces.Services;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(
        string email,
        string resetLink);

    Task SendOrderEmailAsync(
        string email,
        string message);
}