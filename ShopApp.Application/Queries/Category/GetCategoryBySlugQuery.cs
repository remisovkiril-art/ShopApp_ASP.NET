using MediatR;
using ShopApplication.DTOs.CategoryDTOs;
using ShopApplication.Interfaces.Repository;
using CategoryEntity = ShopDomain.Models.Category;

namespace ShopApplication.Queries.Category;

public class GetCategoryBySlugQuery : IRequest<CategoryReadDTO?>
{
    public string Slug { get; }

    public GetCategoryBySlugQuery(string slug)
    {
        Slug = slug;
    }
}

public class GetCategoryBySlugQueryHandler
    : IRequestHandler<GetCategoryBySlugQuery, CategoryReadDTO?>
{
    private readonly ICategoryRepository _repository;

    public GetCategoryBySlugQueryHandler(
        ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoryReadDTO?> Handle(
        GetCategoryBySlugQuery request,
        CancellationToken cancellationToken)
    {
        CategoryEntity? category =
            await _repository.GetCategoryBySlugAsync(
                request.Slug,
                cancellationToken);

        if (category == null)
        {
            return null;
        }

        return new CategoryReadDTO
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Url = category.Url,
            IsActive = category.IsActive,
            ParentId = category.ParentId,
            Products = category.Products
                .Select(product => product.Id)
                .ToList()
        };
    }
}