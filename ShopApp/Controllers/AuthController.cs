using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApplication.DTOs.UserDTOs;
using ShopApplication.Interfaces.Services;
using System.Security.Claims;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(
    IAuthService authService,
    IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;

    [HttpPost]
    public async Task<IActionResult> RegisterUser(
        [FromBody] UserCreateDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(
            dto,
            cancellationToken);

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
    public async Task<IActionResult> Login(
        [FromBody] UserLoginDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(
            dto,
            cancellationToken);

        if (result == null || result.User == null)
        {
            return Unauthorized(
                "Неверный email или пароль.");
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
    public async Task<IActionResult> RefreshToken(
        CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue(
                "refreshToken",
                out var oldRefreshToken))
        {
            return Unauthorized(
                "Refresh token отсутствует в куках.");
        }

        var result = await authService.RefreshTokensAsync(
            oldRefreshToken,
            cancellationToken);

        if (result.AccessToken == null)
        {
            return Unauthorized(
                "Невалидный или просроченный refresh token.");
        }

        SetRefreshTokenCookie(result.NewRefreshToken!);

        return Ok(new
        {
            token = result.AccessToken,
            refreshToken = result.NewRefreshToken
        });
    }

    [AllowAnonymous]
    [HttpGet("login-google")]
    public IActionResult LoginGoogle()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(ExternalResponse))
        };

        return Challenge(
            properties,
            GoogleDefaults.AuthenticationScheme);
    }

    [AllowAnonymous]
    [HttpGet("external-response")]
    public async Task<IActionResult> ExternalResponse(
        CancellationToken cancellationToken)
    {
        var result =
            await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

        if (!result.Succeeded)
        {
            return BadRequest(
                "Помилка зовнішньої аутентифікації.");
        }

        var claims =
            result.Principal?.Identities
                .FirstOrDefault()?
                .Claims;

        var email =
            claims?
                .FirstOrDefault(
                    c => c.Type == ClaimTypes.Email)?
                .Value;

        var name =
            claims?
                .FirstOrDefault(
                    c => c.Type == ClaimTypes.Name)?
                .Value;

        var providerId =
            claims?
                .FirstOrDefault(
                    c => c.Type == ClaimTypes.NameIdentifier)?
                .Value;

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(providerId))
        {
            return BadRequest(
                "Google не повернув необхідні дані.");
        }

        var resultAuth =
            await authService.ExternalLoginAsync(
                email,
                name ?? string.Empty,
                providerId,
                "google",
                cancellationToken);

        if (resultAuth == null ||
            resultAuth.User == null)
        {
            return BadRequest(
                "Не вдалося створити або авторизувати користувача.");
        }

        SetRefreshTokenCookie(
            resultAuth.RefreshToken!);

        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        var frontendUrl =
            "http://localhost:3001/google-callback";

        var redirectUrl =
            $"{frontendUrl}" +
            $"#token={Uri.EscapeDataString(resultAuth.Token!)}" +
            $"&refreshToken={Uri.EscapeDataString(resultAuth.RefreshToken!)}" +
            $"&email={Uri.EscapeDataString(resultAuth.User.Email)}";

        return Redirect(redirectUrl);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok("Вихід успішний.");
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
                    _configuration.GetValue<int>(
                        "Jwt:ExpiresRefreshTokenDay"))
            });
    }
}

