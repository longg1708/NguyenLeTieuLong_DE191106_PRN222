using GalleryRepositories;
using GalleryBusiness.Models;
namespace GalleryDataAccess;

public class BookService : IBookService
{
    private readonly IBookRepository _repo;
    public BookService(IBookRepository repo) => _repo = repo;

    public async Task<List<Book>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<List<Book>> SearchAsync(string? keyword) => await _repo.SearchAsync(keyword);
    public async Task<Book?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

    public async Task<(bool Success, string Message)> AddBookAsync(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title))
            return (false, "Title cannot be empty.");
        if (string.IsNullOrWhiteSpace(book.Author))
            return (false, "Author cannot be empty.");
        if (book.Quantity < 0)
            return (false, "Quantity must be >= 0.");
        if (!string.IsNullOrWhiteSpace(book.Isbn))
        {
            var existing = await _repo.GetByIsbnAsync(book.Isbn);
            if (existing != null) return (false, "ISBN already exists.");
        }
        book.CreatedAt = DateTime.Now;
        await _repo.AddAsync(book);
        return (true, "Book added successfully!");
    }

    public async Task<(bool Success, string Message)> UpdateBookAsync(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title))
            return (false, "Title cannot be empty.");
        if (string.IsNullOrWhiteSpace(book.Author))
            return (false, "Author cannot be empty.");
        if (book.Quantity < 0)
            return (false, "Quantity must be >= 0.");
        if (!string.IsNullOrWhiteSpace(book.Isbn))
        {
            var existing = await _repo.GetByIsbnAsync(book.Isbn);
            if (existing != null && existing.Id != book.Id)
                return (false, "ISBN already exists.");
        }
        await _repo.UpdateAsync(book);
        return (true, "Book updated successfully!");
    }

    public async Task<(bool Success, string Message)> DeleteBookAsync(int id)
    {
        var book = await _repo.GetByIdAsync(id);
        if (book == null) return (false, "Book not found.");
        await _repo.DeleteAsync(id);
        return (true, "Book deleted successfully!");
    }
}