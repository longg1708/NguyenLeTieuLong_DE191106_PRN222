using GalleryBusiness.Models;
using Microsoft.EntityFrameworkCore;
namespace GalleryRepositories;

public class BookRepository : IBookRepository
{
    private readonly IDbContextFactory<LibraryDbContext> _factory;
    public BookRepository(IDbContextFactory<LibraryDbContext> factory) => _factory = factory;

    public async Task<List<Book>> GetAllAsync()
    {
        using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Books.ToListAsync();
    }

    // 1 keyword tìm cả title + author + category
    public async Task<List<Book>> SearchAsync(string? keyword)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        if (string.IsNullOrWhiteSpace(keyword))
            return await ctx.Books.ToListAsync();
        return await ctx.Books.Where(b =>
            b.Title.Contains(keyword) ||
            b.Author.Contains(keyword) ||
            (b.Category != null && b.Category.Contains(keyword))
        ).ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Books.FindAsync(id);
    }

    public async Task<Book?> GetByIsbnAsync(string isbn)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Books.FirstOrDefaultAsync(b => b.Isbn == isbn);
    }

    public async Task AddAsync(Book book)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        ctx.Books.Add(book);
        await ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        ctx.Books.Update(book);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var ctx = await _factory.CreateDbContextAsync();
        var book = await ctx.Books.FindAsync(id);
        if (book != null)
        {
            ctx.Books.Remove(book);
            await ctx.SaveChangesAsync();
        }
    }

    public Task SaveAsync() => Task.CompletedTask;
}