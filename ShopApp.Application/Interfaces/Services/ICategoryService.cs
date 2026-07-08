using ShopApplication.DTOs.CategoryDTOs;

namespace ShopApplication.Interfaces.Services;

public interface ICategoryService
{
    Task<int?> CreateCategoryAsync(CategoryCreateDTO dto);
    Task<List<CategoryReadDTO>?> GetAllCategoriesAsync();
    Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
}
