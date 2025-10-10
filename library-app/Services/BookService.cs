using LibraryApp.Data;
using LibraryApp.Dtos;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class BookService : IBookService
{
    private readonly LibraryContext _db;

    public BookService(LibraryContext db)
    {
        _db = db;
    }

    public async Task<BookDto> CreateAsync(BookDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            CategoryId = dto.CategoryId,
            IsAvailable = dto.IsAvailable,
            DateCreated = dto.DateCreated == default ? DateTime.UtcNow : dto.DateCreated
        };
        _db.Books.Add(book);
        await _db.SaveChangesAsync();
        dto.BookId = book.BookId;
        return dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _db.Books.FindAsync(id);
        if (book == null) return false;
        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        return await _db.Books.Select(b => new BookDto
        {
            BookId = b.BookId,
            Title = b.Title,
            Author = b.Author,
            CategoryId = b.CategoryId,
            IsAvailable = b.IsAvailable,
            DateCreated = b.DateCreated
        }).ToListAsync();
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var b = await _db.Books.FindAsync(id);
        if (b == null) return null;
        return new BookDto
        {
            BookId = b.BookId,
            Title = b.Title,
            Author = b.Author,
            CategoryId = b.CategoryId,
            IsAvailable = b.IsAvailable,
            DateCreated = b.DateCreated
        };
    }

    public async Task<bool> UpdateAsync(int id, BookDto dto)
    {
        var book = await _db.Books.FindAsync(id);
        if (book == null) return false;
        book.Title = dto.Title;
        book.Author = dto.Author;
        book.CategoryId = dto.CategoryId;
        book.IsAvailable = dto.IsAvailable;
        await _db.SaveChangesAsync();
        return true;
    }
}
