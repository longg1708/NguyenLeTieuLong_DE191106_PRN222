// UserService.cs
using GalleryRepositories;
using GalleryBusiness.Models;
using System.Security.Cryptography;
using System.Text;
namespace GalleryDataAccess;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo) => _repo = repo;

    public async Task<User?> LoginAsync(string email, string password)
    {
        var hashed = HashPassword(password);
        var user = await _repo.GetByEmailAsync(email);
        return (user != null && user.Password == hashed) ? user : null;
    }

    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }
}