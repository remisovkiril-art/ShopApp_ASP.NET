using Microsoft.EntityFrameworkCore;
using ShopApplication.Interfaces.Repository;
using ShopDomain.Models;
using ShopInfrastructure.Data;

namespace ShopInfrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ShopDbContext _context;

    public UserRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task<DeliveryAddress?> AddDeliveryAddressAsync(
        DeliveryAddress address,
        CancellationToken cancellationToken)
    {
        await _context.DeliveryAddresses.AddAsync(
            address,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return address;
    }

    public async Task<List<DeliveryAddress>> GetDeliveryAddressesAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _context.DeliveryAddresses
            .Where(a => a.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}