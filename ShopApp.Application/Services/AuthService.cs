using AutoMapper;
using ShopApplication.DTOs;
using ShopApplication.DTOs.UserDTOs;
using ShopApplication.Interfaces.Helpers;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopDomain.Models;

namespace ShopApplication.Services;

public class AuthService(
    IMapper mapper,
    IAuthRepository repository,
    IHashHelper hashHelper,
    IJWTService jwtService) : IAuthService
{
    public async Task<AuthResponseDTO?> RegisterAsync(UserCreateDTO dto)
    {
        var isExist = await repository.IsExistEmailAsync(dto.Email);

        if (isExist)
            return null;

        var hash = hashHelper.Hash(dto.Password);

        var user = mapper.Map<User>(dto);

        var registerUser = await repository.RegisterUserAsync(user, hash);

        if (registerUser == null)
            return null;

        var token = jwtService.GenerateAccessToken(
            mapper.Map<UserLoginDTO>(registerUser),
            registerUser.Role.ToString());

        var (refreshTokenString, days) = jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshTokenString,
            UserId = registerUser.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(days),
            IsRevoked = false
        };

        await repository.SaveRefreshTokenAsync(refreshTokenEntity);

        return new AuthResponseDTO
        {
            User = mapper.Map<UserReadDTO>(registerUser),
            Token = token,
            RefreshToken = refreshTokenString
        };
    }

    public async Task<(string? AccessToken, string? NewRefreshToken)> RefreshTokensAsync(string oldRefreshToken)
    {
        var dbToken = await repository.GetRefreshTokenAsync(oldRefreshToken);

        if (dbToken == null || dbToken.ExpiresAt < DateTime.UtcNow)
            return (null, null);

        dbToken.IsRevoked = true;

        var userLoginDto = new UserLoginDTO
        {
            Email = dbToken.User?.Email ?? string.Empty
        };

        var newAccessToken = jwtService.GenerateAccessToken(
            userLoginDto,
            dbToken.User?.Role.ToString() ?? "User");

        var (newRefreshTokenString, days) = jwtService.GenerateRefreshToken();

        var newRefreshTokenEntity = new RefreshToken
        {
            Token = newRefreshTokenString,
            UserId = dbToken.UserId,
            ExpiresAt = DateTime.UtcNow.AddDays(days),
            IsRevoked = false
        };

        await repository.SaveRefreshTokenAsync(newRefreshTokenEntity);

        return (newAccessToken, newRefreshTokenString);
    }
}