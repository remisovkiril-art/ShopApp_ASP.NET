using Microsoft.AspNetCore.Http;
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
            imageUrl = await _imageService.SaveFileAsync(request.Image);
        }

        var createdDto = new CategoryCreateDTO
        {
            Name = request.Name,
            Url = imageUrl,
            Slug = request.Slug,
            ParentId = request.ParentId
        };

        int? id = await _categoryService.CreateCategoryAsync(createdDto);
        return Ok($"Category created {id}");
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }
}

