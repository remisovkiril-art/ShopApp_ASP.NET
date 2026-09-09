using Microsoft.EntityFrameworkCore;
using ShopApplication.Interfaces.Repository;
using ShopDomain.Models;
using ShopInfrastructure.Data;

namespace ShopInfrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly ShopDbContext _context;

    public AuthRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsExistEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        return await _context.Users
            .AnyAsync(
                u => u.Email == email,
                cancellationToken);
    }

    public async Task<User?> RegisterUserAsync(
        User user,
        string hash,
        CancellationToken cancellationToken)
    {
        user.PasswordHash = hash;

        await _context.Users.AddAsync(
            user,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return await _context.Users
            .FirstOrDefaultAsync(
                u => u.Email == user.Email &&
                     u.PasswordHash == user.PasswordHash,
                cancellationToken);
    }

    public async Task SaveRefreshTokenAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken)
    {
        await _context.RefreshTokens.AddAsync(
            refreshToken,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(
        string token,
        CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(
                rt => rt.Token == token &&
                      !rt.IsRevoked,
                cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                u => u.Email == email,
                cancellationToken);
    }

    public async Task UpdateRefreshTokenAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken)
    {
        _context.RefreshTokens.Update(refreshToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task SavePasswordResetTokenAsync(
        PasswordResetToken token,
        CancellationToken cancellationToken)
    {
        await _context.PasswordResetTokens.AddAsync(
            token,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<PasswordResetToken?> GetPasswordResetTokenAsync(
        string token,
        CancellationToken cancellationToken)
    {
        return await _context.PasswordResetTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(
                t => t.Token == token &&
                     !t.IsUsed,
                cancellationToken);
    }

    public async Task UpdatePasswordResetTokenAsync(
        PasswordResetToken token,
        CancellationToken cancellationToken)
    {
        _context.PasswordResetTokens.Update(token);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}