using Microsoft.AspNetCore.Mvc;
using ShopApplication.DTOs.ProductFeedbackDTOs;
using ShopApplication.Interfaces.Services;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductFeedbackController : ControllerBase
{
    private readonly IProductFeedbackService _feedbackService;

    public ProductFeedbackController(
        IProductFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpPost]
    public async Task<IActionResult> AddFeedback(
        ProductFeedbackCreateDTO dto,
        CancellationToken cancellationToken)
    {
        if (dto.ProductId <= 0)
        {
            return BadRequest("Некорректный ProductId");
        }

        if (string.IsNullOrWhiteSpace(dto.Type))
        {
            return BadRequest("Укажите тип: Review или Question");
        }

        if (string.IsNullOrWhiteSpace(dto.Message))
        {
            return BadRequest("Сообщение не может быть пустым");
        }

        await _feedbackService.AddAsync(
            dto,
            cancellationToken);

        return Ok();
    }
}