using ShopApplication.DTOs.UserDTOs;

namespace ShopApplication.Interfaces.Services;

public interface IAdminService
{
    Task<UserReadDTO?> CreateAdminAsync(AdminCreateDTO dto);
}