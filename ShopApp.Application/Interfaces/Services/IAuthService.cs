using ShopApplication.DTOs;
using ShopApplication.DTOs.UserDTOs;

namespace ShopApplication.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDTO?> RegisterAsync(UserCreateDTO dto);

    Task<(string? AccessToken, string? NewRefreshToken)>
        RefreshTokensAsync(string oldRefreshToken);

    Task<AuthResponseDTO?> LoginAsync(UserLoginDTO dto);

    Task<bool> SendPasswordResetEmailAsync(string email);

    Task<bool> ResetPasswordAsync(string token, string newPassword);
}