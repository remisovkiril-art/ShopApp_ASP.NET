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

    public async Task<int> CreateProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product.Id;
    }

    public Task<List<Product>> GetAllProductsAsync()
    {
        return _context.Products
            .Include(product => product.Images)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<Product?> GetProductByIdAsync(int id)
    {
        return _context.Products
            .Include(product => product.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id);
    }
}