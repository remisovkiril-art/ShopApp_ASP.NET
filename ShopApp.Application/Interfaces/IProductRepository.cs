using ShopDomain.Models;

namespace ShopApplication.Interfaces.Repository;

public interface IProductRepository
{
    Task<int> CreateProductAsync(
        Product product,
        CancellationToken cancellationToken);

    Task<List<Product>> GetAllProductsAsync(
        CancellationToken cancellationToken);

    Task<Product?> GetProductByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task DeleteProductAsync(
        int id,
        CancellationToken cancellationToken);
}
