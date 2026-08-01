using Microsoft.AspNetCore.Mvc;
using ShopApplication.DTOs.UserDTOs;
using ShopApplication.Interfaces.Services;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(
    IAuthService authService,
    IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] UserCreateDTO dto)
    {
        var result = await authService.RegisterAsync(dto);

        if (result == null || result.User == null)
        {
            return NotFound();
        }

        SetRefreshTokenCookie(result.RefreshToken!);

        return Ok(new
        {
            user = result.User,
            token = result.Token,
            refreshToken = result.RefreshToken
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
    {
        var result = await authService.LoginAsync(dto);

        if (result == null || result.User == null)
        {
            return Unauthorized("Неверный email или пароль.");
        }

        SetRefreshTokenCookie(result.RefreshToken!);

        return Ok(new
        {
            user = result.User,
            token = result.Token,
            refreshToken = result.RefreshToken
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var oldRefreshToken))
        {
            return Unauthorized("Refresh token отсутствует в куках.");
        }

        var result = await authService.RefreshTokensAsync(oldRefreshToken);

        if (result.AccessToken == null)
        {
            return Unauthorized("Невалидный или просроченный refresh token.");
        }

        SetRefreshTokenCookie(result.NewRefreshToken!);

        return Ok(new
        {
            token = result.AccessToken,
            refreshToken = result.NewRefreshToken
        });
    }

    private void SetRefreshTokenCookie(string token)
    {
        Response.Cookies.Append(
            "refreshToken",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(
                    _configuration.GetValue<int>("Jwt:ExpiresRefreshTokenDay"))
            });
    }
}