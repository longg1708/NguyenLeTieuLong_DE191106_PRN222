using GalleryBusiness.Models;
namespace GalleryRepositories;

public interface IBorrowRepository
{
    Task<bool> HasBorrowedAsync(int userId, int bookId);
    Task AddAsync(BorrowRecord record);
    Task<List<BorrowRecord>> GetAllWithDetailsAsync();
    Task<List<BorrowRecord>> GetByUserIdAsync(int userId);
    Task ReturnBookAsync(int recordId);
    Task SaveAsync();
}