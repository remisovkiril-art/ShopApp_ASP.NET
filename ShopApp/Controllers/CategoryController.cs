//using Microsoft.AspNetCore.Mvc;
//using ShopApplication.DTOs.CategoryDTOs;
//using ShopApplication.Interfaces.Services;

//namespace ShopApi.Controllers;

//[ApiController]
//[Route("api/v1/[controller]")]
//public class CategoryController : ControllerBase
//{
//    private readonly ICategoryService _categoryService;

//    public CategoryController(ICategoryService categoryService)
//    {
//        _categoryService = categoryService;
//    }

//    [HttpPost]
//    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDTO dto)
//    {
//        int? id = await _categoryService.CreateCategoryAsync(dto);
//        return Ok($"Category created {id}");
//    }
//    [HttpGet]
//    public async Task<IActionResult> GetCategories()
//    {
//        var categories = await _categoryService.GetAllCategoriesAsync();
//        return Ok(categories);
//    }
//}
