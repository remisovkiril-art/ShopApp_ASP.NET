using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApplication.DTOs.UserDTOs;
using ShopApplication.Interfaces.Services;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAdmin([FromBody] AdminCreateDTO dto)
    {
        var result = await adminService.CreateAdminAsync(dto);

        if (result == null)
        {
            return BadRequest(
                "Пользователь с таким email уже существует или указана недопустимая роль"
            );
        }

        return Ok(result);
    }
}