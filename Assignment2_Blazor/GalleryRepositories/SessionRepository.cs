using GalleryBusiness.Models;
using Microsoft.EntityFrameworkCore;
namespace GalleryRepositories;

public class SessionRepository : ISessionRepository
{
    private readonly IDbContextFactory<LibraryDbContext> _factory;
    public SessionRepository(IDbContextFactory<LibraryDbContext> factory) => _factory = factory;

    public async Task AddAsync(Session session)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        ctx.Sessions.Add(session);
        await ctx.SaveChangesAsync();
    }

    public async Task<Session?> GetByIdAsync(string sessionId)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Sessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);
    }

    public async Task DeleteAsync(string sessionId)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        var session = await ctx.Sessions.FindAsync(sessionId);
        if (session != null)
        {
            ctx.Sessions.Remove(session);
            await ctx.SaveChangesAsync();
        }
    }

    public Task SaveAsync() => Task.CompletedTask;
}