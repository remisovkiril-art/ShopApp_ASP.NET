using ShopApplication.DTOs.UserDTOs;

namespace ShopApplication.Interfaces.Services;

public interface IJWTService
{
    string GenerateAccessToken(UserLoginDTO userLoginDto, string role);

    (string, int) GenerateRefreshToken();
}