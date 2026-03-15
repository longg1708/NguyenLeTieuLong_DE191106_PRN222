// IUserService.cs
using GalleryBusiness.Models;
namespace GalleryDataAccess;

public interface IUserService
{
    Task<User?> LoginAsync(string email, string password);
}