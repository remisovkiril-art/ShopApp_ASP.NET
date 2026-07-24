using ShopApplication.DTOs.CategoryDTOs;

namespace ShopApplication.Interfaces.Services;

public interface ICategoryService
{
    Task<int?> CreateCategoryAsync(CategoryCreateDTO dto);

    Task<List<CategoryReadDTO>?> GetAllCategoriesAsync();

    Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);

    Task<bool> DeleteCategoryAsync(int id);

    Task<bool> UpdateCategoryAsync(CategoryUpdateDTO dto);
    Task<List<CategoryReadDTO>> GetParentCategoriesAsync(int categoryId);
    Task<List<CategoryReadDTO>> GetChildCategoriesAsync(int categoryId);
    Task<List<CategoryNodeDTO>> GetCategoryTreeAsync();
}
