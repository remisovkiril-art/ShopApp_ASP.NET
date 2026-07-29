using ShopDomain.Models;
using System.Threading.Tasks;

namespace ShopApplication.Interfaces.Repository;

public interface IAuthRepository
{
    Task<User>? RegisterUserAsync(User user, string hash);
    Task<bool> IsExistEmailAsync(string email);
    Task SaveRefreshTokenAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task<User?> GetUserByEmailAsync(string email);
}

