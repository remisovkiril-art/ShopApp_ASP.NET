using Microsoft.EntityFrameworkCore;
using ShopApplication.Interfaces.Repository;
using ShopDomain.Models;
using ShopInfrastructure.Data;

namespace ShopInfrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ShopDbContext _context;

    public ProductRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateProductAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(
            product,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return product.Id;
    }

    public Task<List<Product>> GetAllProductsAsync(
        CancellationToken cancellationToken)
    {
        return _context.Products
            .Include(product => product.Images)
            .Where(product => product.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Product?> GetProductByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return _context.Products
            .Include(product => product.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                product =>
                    product.Id == id &&
                    product.IsActive,
                cancellationToken);
    }

    public async Task DeleteProductAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(
                product => product.Id == id,
                cancellationToken);

        if (product != null)
        {
            product.IsActive = false;

            await _context.SaveChangesAsync(
                cancellationToken);
        }
    }
}

