using ShopDomain.Models;

namespace ShopApplication.Interfaces.Repository;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<int?> CreateCategoryAsync(Category category);
}

