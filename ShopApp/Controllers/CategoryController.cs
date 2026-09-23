using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Interfaces;
using ShopApi.Requests.Categories;
using ShopApplication.Commands.Category;
using ShopApplication.DTOs.CategoryDTOs;
using ShopApplication.Interfaces.Services;
using ShopApplication.Queries.Category;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IImageService _imageService;
    private readonly IMediator _mediator;
    private readonly IValidator<CategoryCreateDTO> _validator;

    public CategoryController(
        ICategoryService categoryService,
        IImageService imageService,
        IMediator mediator,
        IValidator<CategoryCreateDTO> validator)
    {
        _categoryService = categoryService;
        _imageService = imageService;
        _mediator = mediator;
        _validator = validator;
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

        CategoryCreateDTO dto = new CategoryCreateDTO
        {
            Name = request.Name,
            Url = imageUrl,
            Slug = request.Slug,
            ParentId = request.ParentId
        };

        var result = await _validator.ValidateAsync(
            dto,
            cancellationToken);

        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

        int? id = await _mediator.Send(
            new CreateCategoryCommand(dto),
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
        List<CategoryReadDTO>? categories =
            await _categoryService.GetAllCategoriesAsync(
                cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryReadDTO>> GetCategoryById(
        int id,
        CancellationToken cancellationToken)
    {
        CategoryReadDTO? dto = await _mediator.Send(
            new GetCategoryByIdQuery(id),
            cancellationToken);

        if (dto == null)
        {
            return NotFound();
        }

        return Ok(dto);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<CategoryReadDTO>> GetCategoryBySlug(
        string slug,
        CancellationToken cancellationToken)
    {
        CategoryReadDTO? dto = await _mediator.Send(
            new GetCategoryBySlugQuery(slug),
            cancellationToken);

        if (dto == null)
        {
            return NotFound();
        }

        return Ok(dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(
        int id,
        [FromBody] CategoryUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.Id = id;

        bool result =
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
        bool result =
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
        List<CategoryReadDTO> parents =
            await _categoryService.GetParentCategoriesAsync(
                id,
                cancellationToken);

        return Ok(parents);
    }

    [HttpGet("{id:int}/children")]
    public async Task<ActionResult<List<CategoryReadDTO>>> GetChildren(
        int id,
        CancellationToken cancellationToken)
    {
        List<CategoryReadDTO> children =
            await _categoryService.GetChildCategoriesAsync(
                id,
                cancellationToken);

        return Ok(children);
    }

    [HttpGet("tree")]
    public async Task<ActionResult<List<CategoryNodeDTO>>> GetTree(
        CancellationToken cancellationToken)
    {
        List<CategoryNodeDTO> tree =
            await _categoryService.GetCategoryTreeAsync(
                cancellationToken);

        return Ok(tree);
    }
}
