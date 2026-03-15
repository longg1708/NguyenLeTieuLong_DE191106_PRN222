using GalleryBusiness.Models;
namespace GalleryRepositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();
    Task<List<Book>> SearchAsync(string? keyword);
    Task<Book?> GetByIdAsync(int id);
    Task<Book?> GetByIsbnAsync(string isbn);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
    Task SaveAsync();
}