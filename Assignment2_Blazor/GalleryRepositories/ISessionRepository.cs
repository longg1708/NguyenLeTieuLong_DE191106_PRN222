// ISessionRepository.cs
using GalleryBusiness.Models;
namespace GalleryRepositories;

public interface ISessionRepository
{
    Task AddAsync(Session session);
    Task<Session?> GetByIdAsync(string sessionId);
    Task DeleteAsync(string sessionId);
    Task SaveAsync();
}