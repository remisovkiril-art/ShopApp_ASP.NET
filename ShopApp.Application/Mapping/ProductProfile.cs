using AutoMapper;
using ShopApplication.DTOs.ProductDTOs;
using ShopDomain.Models;

namespace ShopApplication.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductReadDTO>()
            .ForMember(dest => dest.ImageUrls,
                opt => opt.MapFrom(src => src.Images.Select(img => img.Url).ToList()));
    }
}
