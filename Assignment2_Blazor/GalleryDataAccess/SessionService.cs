// SessionService.cs
using GalleryRepositories;
using GalleryBusiness.Models;
namespace GalleryDataAccess;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _repo;
    public SessionService(ISessionRepository repo) => _repo = repo;

    public async Task<string> CreateSessionAsync(int userId, string role)
    {
        var sessionId = Guid.NewGuid().ToString();
        var session = new Session
        {
            SessionId = sessionId,
            UserId = userId,
            Role = role,
            ExpiresAt = DateTime.Now.AddHours(2)
        };
        await _repo.AddAsync(session);
        await _repo.SaveAsync();
        return sessionId;
    }

    public async Task<Session?> GetSessionAsync(string sessionId)
    {
        var session = await _repo.GetByIdAsync(sessionId);
        if (session == null || session.ExpiresAt < DateTime.Now)
        {
            if (session != null) await DeleteSessionAsync(sessionId);
            return null;
        }
        return session;
    }

    public async Task DeleteSessionAsync(string sessionId)
    {
        await _repo.DeleteAsync(sessionId);
        await _repo.SaveAsync();
    }
}