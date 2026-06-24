using Microsoft.AspNetCore.Mvc;
using ShopDomain.Models;
namespace ShopApp.practice2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            return Ok(user);
        }
    }
}