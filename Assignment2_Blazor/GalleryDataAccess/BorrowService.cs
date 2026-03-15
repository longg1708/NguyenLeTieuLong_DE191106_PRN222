using GalleryRepositories;
using GalleryBusiness.Models;
namespace GalleryDataAccess;

public class BorrowService : IBorrowService
{
    private readonly IBorrowRepository _borrowRepo;
    private readonly IBookRepository _bookRepo;

    public BorrowService(IBorrowRepository borrowRepo, IBookRepository bookRepo)
    {
        _borrowRepo = borrowRepo;
        _bookRepo = bookRepo;
    }

    public async Task<(bool Success, string Message)> BorrowBookAsync(int userId, int bookId)
    {
        var book = await _bookRepo.GetByIdAsync(bookId);
        if (book == null) return (false, "Book not found.");
        if (book.Quantity <= 0) return (false, "No copies available.");

        var alreadyBorrowed = await _borrowRepo.HasBorrowedAsync(userId, bookId);
        if (alreadyBorrowed) return (false, "You have already borrowed this book.");

        book.Quantity -= 1;
        await _bookRepo.UpdateAsync(book);

        var record = new BorrowRecord
        {
            UserId = userId,
            BookId = bookId,
            BorrowDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(14),
            Returned = false
        };
        await _borrowRepo.AddAsync(record);
        return (true, "Borrowed successfully! Due date: " + record.DueDate.ToString("dd/MM/yyyy"));
    }

    public async Task<List<BorrowRecord>> GetAllRecordsAsync()
        => await _borrowRepo.GetAllWithDetailsAsync();

    public async Task<List<BorrowRecord>> GetMyBorrowsAsync(int userId)
        => await _borrowRepo.GetByUserIdAsync(userId);

    public async Task<(bool Success, string Message)> ReturnBookAsync(int recordId)
    {
        await _borrowRepo.ReturnBookAsync(recordId);
        return (true, "Book returned successfully!");
    }
}