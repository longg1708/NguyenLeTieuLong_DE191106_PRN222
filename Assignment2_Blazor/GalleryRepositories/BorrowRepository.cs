using GalleryBusiness.Models;
using Microsoft.EntityFrameworkCore;
namespace GalleryRepositories;

public class BorrowRepository : IBorrowRepository
{
    private readonly IDbContextFactory<LibraryDbContext> _factory;
    public BorrowRepository(IDbContextFactory<LibraryDbContext> factory) => _factory = factory;

    public async Task<bool> HasBorrowedAsync(int userId, int bookId)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.BorrowRecords
            .AnyAsync(r => r.UserId == userId && r.BookId == bookId && r.Returned == false);
    }

    public async Task AddAsync(BorrowRecord record)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        ctx.BorrowRecords.Add(record);
        await ctx.SaveChangesAsync();
    }

    public async Task<List<BorrowRecord>> GetAllWithDetailsAsync()
    {
        using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.BorrowRecords
            .Include(r => r.User)
            .Include(r => r.Book)
            .ToListAsync();
    }

    public async Task<List<BorrowRecord>> GetByUserIdAsync(int userId)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.BorrowRecords
            .Include(r => r.Book)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task ReturnBookAsync(int recordId)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        var record = await ctx.BorrowRecords.FindAsync(recordId);
        if (record == null || record.Returned == true) return;

        record.Returned = true;
        var book = await ctx.Books.FindAsync(record.BookId);
        if (book != null) book.Quantity += 1;

        await ctx.SaveChangesAsync();
    }

    public Task SaveAsync() => Task.CompletedTask;
}