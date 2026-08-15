using AutoMapper;
using ShopApplication.DTOs;
using ShopApplication.DTOs.UserDTOs;
using ShopApplication.Interfaces.Helpers;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopDomain.Enums;
using ShopDomain.Models;
using System.Security.Cryptography;

namespace ShopApplication.Services;

public class AuthService(
    IMapper mapper,
    IAuthRepository repository,
    IHashHelper hashHelper,
    IJWTService jwtService,
    IEmailService emailService) : IAuthService
{
    public async Task<AuthResponseDTO?> RegisterAsync(
        UserCreateDTO dto)
    {
        var isExist = await repository.IsExistEmailAsync(dto.Email);

        if (isExist)
            return null;

        var hash = hashHelper.Hash(dto.Password);

        var user = mapper.Map<User>(dto);

        var registerUser =
            await repository.RegisterUserAsync(user, hash);

        if (registerUser == null)
            return null;

        var token = jwtService.GenerateAccessToken(
            mapper.Map<UserLoginDTO>(registerUser),
            registerUser.Role.ToString());

        var (refreshTokenString, days) =
            jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshTokenString,
            UserId = registerUser.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(days),
            IsRevoked = false
        };

        await repository.SaveRefreshTokenAsync(
            refreshTokenEntity);

        return new AuthResponseDTO
        {
            User = mapper.Map<UserReadDTO>(registerUser),
            Token = token,
            RefreshToken = refreshTokenString
        };
    }

    public async Task<(string? AccessToken, string? NewRefreshToken)>
        RefreshTokensAsync(string oldRefreshToken)
    {
        var dbToken =
            await repository.GetRefreshTokenAsync(oldRefreshToken);

        if (dbToken == null ||
            dbToken.ExpiresAt < DateTime.UtcNow)
        {
            return (null, null);
        }

        dbToken.IsRevoked = true;

        await repository.UpdateRefreshTokenAsync(dbToken);

        var userLoginDto = new UserLoginDTO
        {
            Email = dbToken.User?.Email ?? string.Empty
        };

        var newAccessToken = jwtService.GenerateAccessToken(
            userLoginDto,
            dbToken.User?.Role.ToString() ?? "User");

        var (newRefreshTokenString, days) =
            jwtService.GenerateRefreshToken();

        var newRefreshTokenEntity = new RefreshToken
        {
            Token = newRefreshTokenString,
            UserId = dbToken.UserId,
            ExpiresAt = DateTime.UtcNow.AddDays(days),
            IsRevoked = false
        };

        await repository.SaveRefreshTokenAsync(
            newRefreshTokenEntity);

        return (
            newAccessToken,
            newRefreshTokenString);
    }

    public async Task<AuthResponseDTO?> LoginAsync(
        UserLoginDTO dto)
    {
        var user =
            await repository.GetUserByEmailAsync(dto.Email);

        if (user == null)
            return null;

        if (!hashHelper.IsValidPassword(
                dto.Password,
                user.PasswordHash))
        {
            return null;
        }

        var token = jwtService.GenerateAccessToken(
            mapper.Map<UserLoginDTO>(user),
            user.Role.ToString());

        var (refreshTokenString, days) =
            jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshTokenString,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(days),
            IsRevoked = false
        };

        await repository.SaveRefreshTokenAsync(
            refreshTokenEntity);

        return new AuthResponseDTO
        {
            User = mapper.Map<UserReadDTO>(user),
            Token = token,
            RefreshToken = refreshTokenString
        };
    }

    public async Task<bool> SendPasswordResetEmailAsync(
        string email)
    {
        var user =
            await repository.GetUserByEmailAsync(email);

        if (user == null)
            return false;

        if (user.Role != UserRole.Admin &&
            user.Role != UserRole.Moderator)
        {
            return false;
        }

        var tokenBytes =
            RandomNumberGenerator.GetBytes(32);

        var token =
            Convert.ToBase64String(tokenBytes);

        var resetToken = new PasswordResetToken
        {
            UserId = user.Id,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            IsUsed = false
        };

        await repository.SavePasswordResetTokenAsync(
            resetToken);

        var resetLink =
            $"https://localhost:7100/api/v1/Password/reset?token={Uri.EscapeDataString(token)}";

        await emailService.SendPasswordResetEmailAsync(
            user.Email,
            resetLink);

        return true;
    }

    public async Task<bool> ResetPasswordAsync(
        string token,
        string newPassword)
    {
        var resetToken =
            await repository.GetPasswordResetTokenAsync(token);

        if (resetToken == null)
            return false;

        if (resetToken.IsUsed ||
            resetToken.ExpiresAt < DateTime.UtcNow)
        {
            return false;
        }

        var user = resetToken.User;

        if (user == null)
            return false;

        if (user.Role != UserRole.Admin &&
            user.Role != UserRole.Moderator)
        {
            return false;
        }

        user.PasswordHash =
            hashHelper.Hash(newPassword);

        resetToken.IsUsed = true;

        await repository.UpdatePasswordResetTokenAsync(
            resetToken);

        return true;
    }
}