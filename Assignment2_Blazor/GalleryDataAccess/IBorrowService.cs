using GalleryBusiness.Models;
namespace GalleryDataAccess;

public interface IBorrowService
{
    Task<(bool Success, string Message)> BorrowBookAsync(int userId, int bookId);
    Task<List<BorrowRecord>> GetAllRecordsAsync();
    Task<List<BorrowRecord>> GetMyBorrowsAsync(int userId);
    Task<(bool Success, string Message)> ReturnBookAsync(int recordId);
}