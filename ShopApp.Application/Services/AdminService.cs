using AutoMapper;
using ShopApplication.DTOs.UserDTOs;
using ShopApplication.Interfaces.Helpers;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopDomain.Models;

namespace ShopApplication.Services;

public class AdminService(
    IAuthRepository repository,
    IHashHelper hashHelper,
    IMapper mapper) : IAdminService
{
    public async Task<UserReadDTO?> CreateAdminAsync(
        AdminCreateDTO dto,
        CancellationToken cancellationToken)
    {
        var isExist = await repository.IsExistEmailAsync(
            dto.Email,
            cancellationToken);

        if (isExist)
            return null;

        if (dto.Role != ShopDomain.Enums.UserRole.Admin &&
            dto.Role != ShopDomain.Enums.UserRole.Moderator)
        {
            return null;
        }

        var user = new User
        {
            Email = dto.Email,
            Role = dto.Role,
            IsActive = true
        };

        var hash = hashHelper.Hash(dto.Password);

        var createdUser =
            await repository.RegisterUserAsync(
                user,
                hash,
                cancellationToken);

        if (createdUser == null)
            return null;

        return mapper.Map<UserReadDTO>(createdUser);
    }
}