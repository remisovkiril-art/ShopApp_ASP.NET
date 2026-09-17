using ShopDomain.Models;

namespace ShopApplication.Interfaces.Repository;

public interface IUserRepository
{
    Task<DeliveryAddress?> AddDeliveryAddressAsync(
        DeliveryAddress address,
        CancellationToken cancellationToken);

    Task<List<DeliveryAddress>> GetDeliveryAddressesAsync(
        Guid userId,
        CancellationToken cancellationToken);
}