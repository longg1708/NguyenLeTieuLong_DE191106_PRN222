// ISessionService.cs
using GalleryBusiness.Models;
namespace GalleryDataAccess;

public interface ISessionService
{
    Task<string> CreateSessionAsync(int userId, string role);
    Task<Session?> GetSessionAsync(string sessionId);
    Task DeleteSessionAsync(string sessionId);
}