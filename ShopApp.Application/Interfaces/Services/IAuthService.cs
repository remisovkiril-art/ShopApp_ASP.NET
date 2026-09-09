using ShopApplication.DTOs;
using ShopApplication.DTOs.UserDTOs;

namespace ShopApplication.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDTO?> RegisterAsync(
        UserCreateDTO dto,
        CancellationToken cancellationToken);

    Task<(string? AccessToken, string? NewRefreshToken)>
        RefreshTokensAsync(
            string oldRefreshToken,
            CancellationToken cancellationToken);

    Task<AuthResponseDTO?> LoginAsync(
        UserLoginDTO dto,
        CancellationToken cancellationToken);

    Task<bool> SendPasswordResetEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task<bool> ResetPasswordAsync(
        string token,
        string newPassword,
        CancellationToken cancellationToken);
}