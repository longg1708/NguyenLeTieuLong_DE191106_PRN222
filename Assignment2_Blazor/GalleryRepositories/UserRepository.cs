using GalleryBusiness.Models;
using Microsoft.EntityFrameworkCore;
namespace GalleryRepositories;

public class UserRepository : IUserRepository
{
    private readonly IDbContextFactory<LibraryDbContext> _factory;
    public UserRepository(IDbContextFactory<LibraryDbContext> factory) => _factory = factory;

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Users.FindAsync(id);
    }
}