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

    /// <summary>
    /// Retrieves all books.
    /// </summary>
    /// <returns>A list of all books.</returns>
    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        return await _db.Books.Select(b => new BookDto(b)).ToListAsync();
    }

    /// <summary>
    /// Retrieves all available books.
    /// </summary>
    /// <returns>A list of all available books.</returns>
    public async Task<IEnumerable<BookDto>> GetAvailableBooksAsync()
    {
        return await _db.Books
            .Where(b => b.IsAvailable)
            .Select(b => new BookDto(b)).ToListAsync();
    }

    /// <summary>
    /// Retrieves a book by its identifier.
    /// </summary>
    /// <param name="id">The book identifier.</param>
    /// <returns>The book dto if found, otherwise null.</returns>
    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var b = await _db.Books.FindAsync(id);
        if (b == null) return null;
        return new BookDto(b);
    }

    /// <summary>
    /// Creates a new book from the given dto.
    /// </summary>
    /// <param name="dto">The book dto.</param>
    /// <returns>The created book dto.</returns>
    public async Task<BookDto> CreateAsync(BookDto dto)
    {
        var book = new Book(dto);
        _db.Books.Add(book);
        await _db.SaveChangesAsync();
        dto.BookId = book.BookId;
        return dto;
    }

    /// <summary>
    /// Deletes a book by its identifier.
    /// </summary>
    /// <param name="id">The book identifier.</param>
    /// <returns>true if the book is found and deleted, otherwise false.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _db.Books.FindAsync(id);
        if (book == null) return false;
        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Updates a book by its identifier.
    /// </summary>
    /// <param name="id">The book identifier.</param>
    /// <param name="dto">The book dto.</param>
    /// <returns>true if the book is found and updated, otherwise false.</returns>
    public async Task<bool> UpdateAsync(int id, BookDto dto)
    {
        var book = await _db.Books.FindAsync(id);
        // if book is on loan, it cannot be marked as available
        if (book == null || (await IsBookOnLoanAsync(id) && !dto.IsAvailable)) return false;

        book.UpdateFromDto(dto);
        await _db.SaveChangesAsync();
        return true;
    }

    // private method to check if book is on loan
    private async Task<bool> IsBookOnLoanAsync(int bookId)
    {
        return await _db.Loans.AnyAsync(l => l.BookId == bookId && l.ReturnDate == null);
    }
}
