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

    public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
    {
        CategoryReadDTO? dto = null;

        var category = await _repository.GetCategoryByIdAsync(id);

        if (category != null)
        {
            dto = _mapper.Map<CategoryReadDTO>(category);
        }

        return dto;
    }

    public async Task<List<CategoryReadDTO>?> GetAllCategoriesAsync()
    {
        var cache = await _cacheService.GetAsync<List<CategoryReadDTO>>("Categories");

        if (cache != null)
        {
            return cache;
        }

        List<Category> categories = await _repository.GetAllCategoriesAsync();

        List<CategoryReadDTO>? dtos = null;

        if (categories != null && categories.Count > 0)
        {
            dtos = _mapper.Map<List<CategoryReadDTO>>(categories);

            await _cacheService.SetAsync("Categories", dtos, null);
        }

        return dtos;
    }

    public async Task<int?> CreateCategoryAsync(CategoryCreateDTO dto)
    {
        var category = _mapper.Map<Category>(dto);

        category.IsActive = true;
        category.CreatedAt = DateTime.UtcNow;
        category.UpdatedAt = DateTime.UtcNow;

        var result = await _repository.CreateCategoryAsync(category);

        await _cacheService.RemoveAsync("Categories");

        return result;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var result = await _repository.DeleteCategoryAsync(id);

        if (result)
        {
            await _cacheService.RemoveAsync("Categories");
        }

        return result;
    }

    public async Task<bool> UpdateCategoryAsync(CategoryUpdateDTO dto)
    {
        var category = await _repository.GetCategoryByIdAsync(dto.Id);

        if (category == null)
            return false;

        category.Name = dto.Name;
        category.Slug = dto.Slug;
        category.Url = dto.Url;
        category.ParentId = dto.ParentId;
        category.UpdatedAt = DateTime.UtcNow;

        var result = await _repository.UpdateCategoryAsync(category);

        if (result)
        {
            await _cacheService.RemoveAsync("Categories");
        }

        return result;
    }

    public async Task<List<CategoryReadDTO>> GetParentCategoriesAsync(int categoryId)
    {
        var allCategories = await _repository.GetAllCategoriesAsync();
        var parents = new List<Category>();

        var current = allCategories.FirstOrDefault(c => c.Id == categoryId);

        while (current != null && current.ParentId != null)
        {
            current = allCategories.FirstOrDefault(c => c.Id == current.ParentId);

            if (current != null)
            {
                parents.Add(current);
            }
        }

        return _mapper.Map<List<CategoryReadDTO>>(parents);
    }

    public async Task<List<CategoryReadDTO>> GetChildCategoriesAsync(int categoryId)
    {
        var allCategories = await _repository.GetAllCategoriesAsync();
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

    public async Task<List<CategoryNodeDTO>> GetCategoryTreeAsync()
    {
        var allCategories = await _repository.GetAllCategoriesAsync();

        var allNodes = allCategories.Select(c => new CategoryNodeDTO
        {
            Id = c.Id,
            Name = c.Name,
            Slug = c.Slug,
            ParentId = c.ParentId,
            Children = new List<CategoryNodeDTO>()
        }).ToList();

        var rootNodes = new List<CategoryNodeDTO>();

        foreach (var node in allNodes)
        {
            if (node.ParentId == null)
            {
                rootNodes.Add(node);
            }
            else
            {
                var parent = allNodes.FirstOrDefault(p => p.Id == node.ParentId);

                if (parent != null)
                {
                    parent.Children.Add(node);
                }
            }
        }

        return rootNodes;
    }
}