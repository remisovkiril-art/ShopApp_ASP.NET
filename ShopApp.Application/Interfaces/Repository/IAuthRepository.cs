using ShopDomain.Models;

namespace ShopApplication.Interfaces.Repository;

public interface IAuthRepository
{
    Task<bool> IsExistEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task<User?> RegisterUserAsync(
        User user,
        string hash,
        CancellationToken cancellationToken);

    Task SaveRefreshTokenAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken);

    Task<RefreshToken?> GetRefreshTokenAsync(
        string token,
        CancellationToken cancellationToken);

    Task<User?> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task UpdateRefreshTokenAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken);

    Task SavePasswordResetTokenAsync(
        PasswordResetToken token,
        CancellationToken cancellationToken);

    Task<PasswordResetToken?> GetPasswordResetTokenAsync(
        string token,
        CancellationToken cancellationToken);

    Task UpdatePasswordResetTokenAsync(
        PasswordResetToken token,
        CancellationToken cancellationToken);
}