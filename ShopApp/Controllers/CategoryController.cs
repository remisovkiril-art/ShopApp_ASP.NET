using Microsoft.AspNetCore.Mvc;
using ShopApi.Interfaces;
using ShopApi.Requests.Categories;
using ShopApplication.DTOs.CategoryDTOs;
using ShopApplication.Interfaces.Services;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IImageService _imageService;

    public CategoryController(ICategoryService categoryService, IImageService imageService)
    {
        _categoryService = categoryService;
        _imageService = imageService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequest request)
    {
        string imageUrl = string.Empty;

        if (request.Image != null)
        {
            imageUrl = (await _imageService.SaveFileAsync(request.Image)) ?? string.Empty;
        }

        var createdDto = new CategoryCreateDTO
        {
            Name = request.Name,
            Url = imageUrl,
            Slug = request.Slug,
            ParentId = request.ParentId
        };

        var id = await _categoryService.CreateCategoryAsync(createdDto);

        return CreatedAtAction(
            nameof(GetCategoryById),
            new { id },
            new { id });
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category == null)
        {
            return NotFound("Категория не найдена.");
        }

        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDTO dto)
    {
        dto.Id = id;

        var result = await _categoryService.UpdateCategoryAsync(dto);

        if (!result)
        {
            return NotFound("Категория не найдена.");
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);

        if (!result)
        {
            return NotFound("Категория не найдена.");
        }

        return NoContent();
    }
}

