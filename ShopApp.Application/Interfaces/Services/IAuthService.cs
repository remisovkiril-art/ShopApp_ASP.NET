using ShopApplication.DTOs;
using ShopApplication.DTOs.UserDTOs;
using System.Threading.Tasks;

namespace ShopApplication.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDTO> RegisterAsync(UserCreateDTO dto);
    Task<(string? AccessToken, string? NewRefreshToken)> RefreshTokensAsync(string oldRefreshToken);
}
