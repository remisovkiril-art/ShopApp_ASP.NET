using ShopApplication.DTOs.CategoryDTOs;

namespace ShopApplication.Interfaces.Services;

public interface ICategoryService
{
    Task<int?> CreateCategoryAsync(
        CategoryCreateDTO dto,
        CancellationToken cancellationToken);

    Task<List<CategoryReadDTO>?> GetAllCategoriesAsync(
        CancellationToken cancellationToken);

    Task<CategoryReadDTO?> GetCategoryByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<bool> DeleteCategoryAsync(
        int id,
        CancellationToken cancellationToken);

    Task<bool> UpdateCategoryAsync(
        CategoryUpdateDTO dto,
        CancellationToken cancellationToken);

    Task<List<CategoryReadDTO>> GetParentCategoriesAsync(
        int categoryId,
        CancellationToken cancellationToken);

    Task<List<CategoryReadDTO>> GetChildCategoriesAsync(
        int categoryId,
        CancellationToken cancellationToken);

    Task<List<CategoryNodeDTO>> GetCategoryTreeAsync(
        CancellationToken cancellationToken);
}