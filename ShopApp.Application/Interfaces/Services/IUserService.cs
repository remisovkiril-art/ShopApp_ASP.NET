using ShopApplication.DTOs.UserDTOs;

namespace ShopApplication.Interfaces.Services;

public interface IUserService
{
    Task<DeliveryAddressReadDTO?> AddDeliveryAddressAsync(
        Guid userId,
        DeliveryAddressCreateDTO dto,
        CancellationToken cancellationToken);

    Task<List<DeliveryAddressReadDTO>> GetDeliveryAddressesAsync(
        Guid userId,
        CancellationToken cancellationToken);
}