using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApplication.DTOs.UserDTOs;
using ShopApplication.Interfaces.Services;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PasswordController(
    IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("forgot")]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordDTO dto,
        CancellationToken cancellationToken)
    {
        var result =
            await authService.SendPasswordResetEmailAsync(
                dto.Email,
                cancellationToken);

        if (!result)
        {
            return BadRequest(
                "Пользователь не найден или не является администратором/модератором.");
        }

        return Ok(
            "Письмо для восстановления пароля отправлено.");
    }

    [AllowAnonymous]
    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordDTO dto,
        CancellationToken cancellationToken)
    {
        var result =
            await authService.ResetPasswordAsync(
                dto.Token,
                dto.NewPassword,
                cancellationToken);

        if (!result)
        {
            return BadRequest(
                "Недействительный или просроченный токен.");
        }

        return Ok(
            "Пароль успешно изменён.");
    }
}