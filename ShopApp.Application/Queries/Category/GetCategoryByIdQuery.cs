using MediatR;
using ShopApplication.DTOs.CategoryDTOs;
using ShopApplication.Interfaces.Repository;
using CategoryEntity = ShopDomain.Models.Category;

namespace ShopApplication.Queries.Category;

public class GetCategoryByIdQuery : IRequest<CategoryReadDTO?>
{
    public int Id { get; }

    public GetCategoryByIdQuery(int id)
    {
        Id = id;
    }
}

public class GetCategoryByIdQueryHandler
    : IRequestHandler<GetCategoryByIdQuery, CategoryReadDTO?>
{
    private readonly ICategoryRepository _repository;

    public GetCategoryByIdQueryHandler(
        ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoryReadDTO?> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        CategoryEntity? category =
            await _repository.GetCategoryByIdAsync(
                request.Id,
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