using AutoMapper;
using ShopApplication.DTOs.UserDTOs;
using ShopApplication.Interfaces.Helpers;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopDomain.Enums;
using ShopDomain.Models;

namespace ShopApplication.Services;

public class AdminService(
    IAuthRepository repository,
    IHashHelper hashHelper,
    IMapper mapper) : IAdminService
{
    public async Task<UserReadDTO?> CreateAdminAsync(AdminCreateDTO dto)
    {
        var isExist = await repository.IsExistEmailAsync(dto.Email);

        if (isExist)
            return null;

        var user = new User
        {
            Email = dto.Email,
            Role = UserRole.Admin,
            IsActive = true
        };

        var hash = hashHelper.Hash(dto.Password);

        var createdUser = await repository.RegisterUserAsync(user, hash);

        if (createdUser == null)
            return null;

        return mapper.Map<UserReadDTO>(createdUser);
    }
}