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

    public async Task<bool> IsExistEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<User?> RegisterUserAsync(User user, string hash)
    {
        user.PasswordHash = hash;

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return await _context.Users
            .FirstOrDefaultAsync(
                u => u.Email == user.Email &&
                     u.PasswordHash == user.PasswordHash);
    }

    public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(
                rt => rt.Token == token &&
                      !rt.IsRevoked);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdateRefreshTokenAsync(
        RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task SavePasswordResetTokenAsync(
        PasswordResetToken token)
    {
        await _context.PasswordResetTokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task<PasswordResetToken?> GetPasswordResetTokenAsync(
        string token)
    {
        return await _context.PasswordResetTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(
                t => t.Token == token &&
                     !t.IsUsed);
    }

    public async Task UpdatePasswordResetTokenAsync(
        PasswordResetToken token)
    {
        _context.PasswordResetTokens.Update(token);
        await _context.SaveChangesAsync();
    }
}