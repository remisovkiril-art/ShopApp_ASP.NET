using ShopApplication.DTOs.CategoryDTOs;
using ShopDomain.Models;

namespace ShopApplication.Interfaces.Services;

public interface ICategoryService
{
    Task<int?> CreateCategoryAsync(CategoryCreateDTO dto);
    Task<List<Category>> GetAllCategoriesAsync();
}
