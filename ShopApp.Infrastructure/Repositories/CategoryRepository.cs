using Microsoft.EntityFrameworkCore;
using ShopApplication.Interfaces.Repository;
using ShopDomain.Models;
using ShopInfrastructure.Data;

namespace ShopInfrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ShopDbContext _context;

    public CategoryRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllCategoriesAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Categories
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Include(category => category.Products)
            .FirstOrDefaultAsync(
                category => category.Id == id,
                cancellationToken);
    }

    public async Task<Category?> GetCategoryBySlugAsync(
        string slug,
        CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Include(category => category.Products)
            .FirstOrDefaultAsync(
                category => category.Slug == slug,
                cancellationToken);
    }

    public async Task<int?> CreateCategoryAsync(
        Category category,
        CancellationToken cancellationToken)
    {
        await _context.Categories.AddAsync(
            category,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return category.Id;
    }

    public async Task<bool> DeleteCategoryAsync(
        int id,
        CancellationToken cancellationToken)
    {
        Category? category = await _context.Categories
            .FirstOrDefaultAsync(
                category => category.Id == id,
                cancellationToken);

        if (category == null)
        {
            return false;
        }

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync(
            cancellationToken);

        return true;
    }

    public async Task<bool> UpdateCategoryAsync(
        Category category,
        CancellationToken cancellationToken)
    {
        _context.Categories.Update(category);

        await _context.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}