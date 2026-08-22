using ShopDomain.Models;

namespace ShopApplication.Interfaces.Repository;

public interface IProductRepository
{
    Task<int> CreateProductAsync(Product product);
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
}
