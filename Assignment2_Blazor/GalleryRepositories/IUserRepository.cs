// IUserRepository.cs
using GalleryBusiness.Models;
namespace GalleryRepositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
}