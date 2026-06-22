using Microsoft.AspNetCore.Mvc;
using Shop.App.practice1.Interfaces;
using Shop.App.practice1.Models;
namespace Shop.App.practice1.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService _categoryService) : ControllerBase
{
    [HttpGet]
    public List<Category> GetCategories()
    {
        return _categoryService.GetAllCategories();
    }
}
