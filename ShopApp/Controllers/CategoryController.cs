using Microsoft.AspNetCore.Authorization;
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

    public CategoryController(
        ICategoryService categoryService,
        IImageService imageService)
    {
        _categoryService = categoryService;
        _imageService = imageService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        [FromForm] CategoryCreateRequest request,
        CancellationToken cancellationToken)
    {
        string imageUrl = string.Empty;

        if (request.Image != null)
        {
            imageUrl = (await _imageService.SaveFileAsync(
                request.Image,
                cancellationToken)) ?? string.Empty;
        }

        var createdDto = new CategoryCreateDTO
        {
            Name = request.Name,
            Url = imageUrl,
            Slug = request.Slug,
            ParentId = request.ParentId
        };

        var id = await _categoryService.CreateCategoryAsync(
            createdDto,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetCategoryById),
            new { id },
            new { id });
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(
        CancellationToken cancellationToken)
    {
        var categories = await _categoryService
            .GetAllCategoriesAsync(cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(
        int id,
        CancellationToken cancellationToken)
    {
        var category = await _categoryService
            .GetCategoryByIdAsync(
                id,
                cancellationToken);

        if (category == null)
        {
            return NotFound("Категория не найдена.");
        }

        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(
        int id,
        [FromBody] CategoryUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.Id = id;

        var result =
            await _categoryService.UpdateCategoryAsync(
                dto,
                cancellationToken);

        if (!result)
        {
            return NotFound("Категория не найдена.");
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _categoryService.DeleteCategoryAsync(
                id,
                cancellationToken);

        if (!result)
        {
            return NotFound("Категория не найдена.");
        }

        return NoContent();
    }

    [HttpGet("{id:int}/parents")]
    public async Task<ActionResult<List<CategoryReadDTO>>> GetParents(
        int id,
        CancellationToken cancellationToken)
    {
        var parents = await _categoryService
            .GetParentCategoriesAsync(
                id,
                cancellationToken);

        return Ok(parents);
    }

    [HttpGet("{id:int}/children")]
    public async Task<ActionResult<List<CategoryReadDTO>>> GetChildren(
        int id,
        CancellationToken cancellationToken)
    {
        var children = await _categoryService
            .GetChildCategoriesAsync(
                id,
                cancellationToken);

        return Ok(children);
    }

    [HttpGet("tree")]
    public async Task<ActionResult<List<CategoryNodeDTO>>> GetTree(
        CancellationToken cancellationToken)
    {
        var tree = await _categoryService
            .GetCategoryTreeAsync(cancellationToken);

        return Ok(tree);
    }
}
