using ShopDomain.Models;

namespace ShopApplication.Interfaces.Repository;

public interface IAuthRepository
{
    Task<User?> RegisterUserAsync(User user, string hash);

    Task<bool> IsExistEmailAsync(string email);

    Task SaveRefreshTokenAsync(RefreshToken refreshToken);

    Task<RefreshToken?> GetRefreshTokenAsync(string token);

    Task<User?> GetUserByEmailAsync(string email);

    Task UpdateRefreshTokenAsync(RefreshToken refreshToken);

    Task SavePasswordResetTokenAsync(PasswordResetToken token);

    Task<PasswordResetToken?> GetPasswordResetTokenAsync(string token);

    Task UpdatePasswordResetTokenAsync(PasswordResetToken token);
}