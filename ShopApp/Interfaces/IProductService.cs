using ShopApplication.DTOs.ProductDTOs;

namespace ShopApplication.Interfaces.Services;

public interface IProductService
{
    Task<int> CreateProductAsync(
        ProductCreateDTO dto,
        CancellationToken cancellationToken);

    Task<List<ProductReadDTO>> GetAllProductsAsync(
        CancellationToken cancellationToken);

    Task<ProductReadDTO?> GetProductByIdAsync(
        int id,
        CancellationToken cancellationToken);
}