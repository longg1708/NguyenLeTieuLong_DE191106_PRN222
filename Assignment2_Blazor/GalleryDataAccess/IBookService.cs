using GalleryBusiness.Models;
namespace GalleryDataAccess;

public interface IBookService
{
    Task<List<Book>> GetAllAsync();
    Task<List<Book>> SearchAsync(string? keyword);
    Task<Book?> GetByIdAsync(int id);
    Task<(bool Success, string Message)> AddBookAsync(Book book);
    Task<(bool Success, string Message)> UpdateBookAsync(Book book);
    Task<(bool Success, string Message)> DeleteBookAsync(int id);
}