using ShopDomain.Models;

namespace ShopApplication.Interfaces.Repository;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoriesAsync(
        CancellationToken cancellationToken);

    Task<Category?> GetCategoryByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<int?> CreateCategoryAsync(
        Category category,
        CancellationToken cancellationToken);

    Task<bool> DeleteCategoryAsync(
        int id,
        CancellationToken cancellationToken);

    Task<bool> UpdateCategoryAsync(
        Category category,
        CancellationToken cancellationToken);
}

