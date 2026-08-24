using Microsoft.AspNetCore.Mvc;
using ShopApplication.DTOs.OrderDTOs;
using ShopApplication.Interfaces.Services;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IQueueService _queueService;

    public OrderController(IQueueService queueService)
    {
        _queueService = queueService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO dto)

    {
        if (dto == null)
        {
            return BadRequest();
        }

        await _queueService.PublishAsync("Orders", dto);

        return Ok();
    }
}
