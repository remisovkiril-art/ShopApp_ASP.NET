using AutoMapper;
using ShopApplication.DTOs.CategoryDTOs;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApplication.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryCreateDTO, Category>();

        CreateMap<Category, CategoryReadDTO>()
            .ForMember(dest => dest.Products,
                opt => opt.MapFrom(src => src.Products.Select(p => p.Id).ToList()));
    }
}