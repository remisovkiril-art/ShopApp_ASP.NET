using ShopDomain.Models;

namespace ShopApplication.Interfaces.Repository;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoriesAsync();

    Task<Category?> GetCategoryByIdAsync(int id);

    Task<int?> CreateCategoryAsync(Category category);

    Task<bool> DeleteCategoryAsync(int id);

    Task<bool> UpdateCategoryAsync(Category category);
}