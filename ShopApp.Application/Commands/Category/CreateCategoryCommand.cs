using MediatR;
using ShopApplication.DTOs.CategoryDTOs;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using CategoryEntity = ShopDomain.Models.Category;

namespace ShopApplication.Commands.Category;

public class CreateCategoryCommand : IRequest<int?>
{
    public CategoryCreateDTO DTO { get; }

    public CreateCategoryCommand(CategoryCreateDTO dto)
    {
        DTO = dto;
    }
}

public class CreateCategoryCommandHandler
    : IRequestHandler<CreateCategoryCommand, int?>
{
    private readonly ICategoryRepository _repository;
    private readonly ICachingService _cacheService;

    public CreateCategoryCommandHandler(
        ICategoryRepository repository,
        ICachingService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<int?> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        CategoryEntity category = new CategoryEntity
        {
            Name = request.DTO.Name,
            Slug = request.DTO.Slug,
            Url = request.DTO.Url,
            ParentId = request.DTO.ParentId,
            IsActive = true
        };

        int? result = await _repository.CreateCategoryAsync(
            category,
            cancellationToken);

        await _cacheService.RemoveAsync("Categories");

        return result;
    }
}