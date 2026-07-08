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

    public CategoryService(ICategoryRepository repository, IMapper _mapper)
    {
        _repository = repository;
        this._mapper = _mapper;
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
        List<Category> categories = await _repository.GetAllCategoriesAsync();
        List<CategoryReadDTO>? dtos = null;
        if (categories != null && categories.Count > 0)
        {
            dtos = _mapper.Map<List<CategoryReadDTO>>(categories);
        }
        return dtos;
    }

    public Task<int?> CreateCategoryAsync(CategoryCreateDTO dto)
    {
        throw new NotImplementedException();
    }
}

