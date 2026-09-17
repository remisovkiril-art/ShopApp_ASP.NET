using AutoMapper;
using ShopApplication.DTOs.UserDTOs;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopDomain.Models;

namespace ShopApplication.Services;

public class UserService(
    IUserRepository repository,
    IMapper mapper) : IUserService
{
    public async Task<DeliveryAddressReadDTO?> AddDeliveryAddressAsync(
        Guid userId,
        DeliveryAddressCreateDTO dto,
        CancellationToken cancellationToken)
    {
        var address = mapper.Map<DeliveryAddress>(dto);

        address.UserId = userId;

        var result = await repository.AddDeliveryAddressAsync(
            address,
            cancellationToken);

        return mapper.Map<DeliveryAddressReadDTO?>(result);
    }

    public async Task<List<DeliveryAddressReadDTO>> GetDeliveryAddressesAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var addresses = await repository.GetDeliveryAddressesAsync(
            userId,
            cancellationToken);

        return mapper.Map<List<DeliveryAddressReadDTO>>(addresses);
    }
}