using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApplication.DTOs.UserDTOs;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using System.Security.Claims;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UserController(
    IUserService userService,
    IAuthRepository authRepository) : ControllerBase
{
    [HttpPost("addresses")]
    public async Task<IActionResult> AddDeliveryAddress(
        [FromBody] DeliveryAddressCreateDTO dto,
        CancellationToken cancellationToken)
    {
        var email =
            User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return Unauthorized();
        }

        var user = await authRepository.GetUserByEmailAsync(
            email,
            cancellationToken);

        if (user == null)
        {
            return Unauthorized();
        }

        var result = await userService.AddDeliveryAddressAsync(
            user.Id,
            dto,
            cancellationToken);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpGet("addresses")]
    public async Task<IActionResult> GetDeliveryAddresses(
        CancellationToken cancellationToken)
    {
        var email =
            User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return Unauthorized();
        }

        var user = await authRepository.GetUserByEmailAsync(
            email,
            cancellationToken);

        if (user == null)
        {
            return Unauthorized();
        }

        var result = await userService.GetDeliveryAddressesAsync(
            user.Id,
            cancellationToken);

        return Ok(result);
    }
}