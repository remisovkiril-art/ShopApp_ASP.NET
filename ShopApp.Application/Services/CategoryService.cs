using AutoMapper;
using ShopApplication.DTOs.CategoryDTOs;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopDomain.Models;

namespace ShopApplication.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICachingService _cacheService;

    public CategoryService(
        ICategoryRepository repository,
        IMapper mapper,
        ICachingService cacheService)
    {
        _repository = repository;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<CategoryReadDTO?> GetCategoryByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"Category:{id}";

        var cachedCategory =
            await _cacheService.GetAsync<CategoryReadDTO>(
                cacheKey);

        if (cachedCategory != null)
        {
            return cachedCategory;
        }

        var category =
            await _repository.GetCategoryByIdAsync(
                id,
                cancellationToken);

        if (category == null)
        {
            return null;
        }

        var dto = _mapper.Map<CategoryReadDTO>(category);

        await _cacheService.SetAsync(
            cacheKey,
            dto,
            null);

        return dto;
    }

    public async Task<List<CategoryReadDTO>?> GetAllCategoriesAsync(
        CancellationToken cancellationToken)
    {
        var cache =
            await _cacheService.GetAsync<List<CategoryReadDTO>>(
                "Categories");

        if (cache != null)
        {
            return cache;
        }

        List<Category> categories =
            await _repository.GetAllCategoriesAsync(
                cancellationToken);

        List<CategoryReadDTO>? dtos = null;

        if (categories != null && categories.Count > 0)
        {
            dtos = _mapper.Map<List<CategoryReadDTO>>(categories);

            await _cacheService.SetAsync(
                "Categories",
                dtos,
                null);
        }

        return dtos;
    }

    public async Task<int?> CreateCategoryAsync(
        CategoryCreateDTO dto,
        CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(dto);

        category.IsActive = true;
        category.CreatedAt = DateTime.UtcNow;
        category.UpdatedAt = DateTime.UtcNow;

        var result =
            await _repository.CreateCategoryAsync(
                category,
                cancellationToken);

        await _cacheService.RemoveAsync("Categories");

        return result;
    }

    public async Task<bool> DeleteCategoryAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _repository.DeleteCategoryAsync(
                id,
                cancellationToken);

        if (result)
        {
            await _cacheService.RemoveAsync("Categories");
            await _cacheService.RemoveAsync($"Category:{id}");
        }

        return result;
    }

    public async Task<bool> UpdateCategoryAsync(
        CategoryUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        var category =
            await _repository.GetCategoryByIdAsync(
                dto.Id,
                cancellationToken);

        if (category == null)
        {
            return false;
        }

        category.Name = dto.Name;
        category.Slug = dto.Slug;
        category.Url = dto.Url;
        category.ParentId = dto.ParentId;
        category.UpdatedAt = DateTime.UtcNow;

        var result =
            await _repository.UpdateCategoryAsync(
                category,
                cancellationToken);

        if (result)
        {
            await _cacheService.RemoveAsync("Categories");
            await _cacheService.RemoveAsync($"Category:{dto.Id}");
        }

        return result;
    }

    public async Task<List<CategoryReadDTO>> GetParentCategoriesAsync(
        int categoryId,
        CancellationToken cancellationToken)
    {
        var allCategories =
            await _repository.GetAllCategoriesAsync(
                cancellationToken);

        var parents = new List<Category>();

        var current =
            allCategories.FirstOrDefault(
                c => c.Id == categoryId);

        while (current != null && current.ParentId != null)
        {
            current =
                allCategories.FirstOrDefault(
                    c => c.Id == current.ParentId);

            if (current != null)
            {
                parents.Add(current);
            }
        }

        return _mapper.Map<List<CategoryReadDTO>>(parents);
    }

    public async Task<List<CategoryReadDTO>> GetChildCategoriesAsync(
        int categoryId,
        CancellationToken cancellationToken)
    {
        var allCategories =
            await _repository.GetAllCategoriesAsync(
                cancellationToken);

        var children = new List<Category>();

        void FindChildren(int parentId)
        {
            var directChildren = allCategories
                .Where(c => c.ParentId == parentId)
                .ToList();

            foreach (var child in directChildren)
            {
                children.Add(child);
                FindChildren(child.Id);
            }
        }

        FindChildren(categoryId);

        return _mapper.Map<List<CategoryReadDTO>>(children);
    }

    public async Task<List<CategoryNodeDTO>> GetCategoryTreeAsync(
        CancellationToken cancellationToken)
    {
        var allCategories =
            await _repository.GetAllCategoriesAsync(
                cancellationToken);

        var allNodes = allCategories
            .Select(c => new CategoryNodeDTO
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                ParentId = c.ParentId,
                Children = new List<CategoryNodeDTO>()
            })
            .ToList();

        var rootNodes = new List<CategoryNodeDTO>();

        foreach (var node in allNodes)
        {
            if (node.ParentId == null)
            {
                rootNodes.Add(node);
            }
            else
            {
                var parent =
                    allNodes.FirstOrDefault(
                        p => p.Id == node.ParentId);

                if (parent != null)
                {
                    parent.Children.Add(node);
                }
            }
        }

        return rootNodes;
    }
}