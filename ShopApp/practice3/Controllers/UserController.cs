using Microsoft.AspNetCore.Mvc;
using Shop.App.Filters;
using ShopDomain.Models;
namespace ShopApp.practice3.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpPost("register")]
    [UserFilter]
    public IActionResult Register(User user)
    {
        return Ok(user);
    }
}