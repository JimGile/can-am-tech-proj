using LibraryApp.Data;
using LibraryApp.Dtos;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore; // Add this using statement

namespace LibraryApp.Services;

public class BookService : IBookService
{
    private readonly LibraryContext _db;

    public BookService(LibraryContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<BookDto>> GetPagedAsync(PagedRequest request, CancellationToken ct = default)
    {
        var query = _db.Books
            .Include(b => b.Category) // Eagerly load the Category
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(b => b.Title.Contains(s) || b.Author.Contains(s));
        }

        bool desc = string.Equals(request.SortDir, "desc", StringComparison.OrdinalIgnoreCase);
        query = request.SortBy?.ToLowerInvariant() switch
        {
            "author" => desc ? query.OrderByDescending(b => b.Author) : query.OrderBy(b => b.Author),
            _ => desc ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
        };

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b => new BookDto // Update the projection to include CategoryName
            {
                BookId = b.BookId,
                Title = b.Title,
                Author = b.Author,
                CategoryId = b.CategoryId,
                CategoryName = b.Category!.Name, // Populate CategoryName
                IsAvailable = b.IsAvailable,
                DateCreated = b.DateCreated,
                RowVersion = b.RowVersion
            })
            .ToListAsync(ct);

        return new PagedResult<BookDto>(items.AsReadOnly(), total, request);
    }

    /// <summary>
    /// Retrieves a book by its identifier.
    /// </summary>
    /// <param name="id">The book identifier.</param>
    /// <returns>The book dto if found, otherwise null.</returns>
    public async Task<BookDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var b = await _db.Books
            .Include(b => b.Category) // Eagerly load the Category
            .FirstOrDefaultAsync(b => b.BookId == id, ct)
            ?? throw new KeyNotFoundException($"Book {id} not found.");
        return new BookDto(b);
    }

    /// <summary>
    /// Creates a new book from the given dto.
    /// </summary>
    /// <param name="dto">The book dto.</param>
    /// <returns>The created book dto.</returns>
    public async Task<BookDto> CreateAsync(BookDto dto, CancellationToken ct = default)
    {
        var book = new Book(dto);
        _db.Books.Add(book);
        await _db.SaveChangesAsync(ct);
        dto.BookId = book.BookId;
        return dto;
    }

    /// <summary>
    /// Deletes a book by its identifier.
    /// </summary>
    /// <param name="id">The book identifier.</param>
    /// <returns>true if the book is found and deleted, otherwise false.</returns>
    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var book = await _db.Books.FindAsync(id, ct) ?? throw new KeyNotFoundException($"Book {id} not found.");
        _db.Books.Remove(book);
        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Updates a book by its identifier.
    /// </summary>
    /// <param name="dto">The book dto.</param>
    /// <returns>true if the book is found and updated, otherwise false.</returns>
    public async Task UpdateAsync(BookDto dto, CancellationToken ct = default)
    {
        var book = await _db.Books.FindAsync(dto.BookId, ct) ?? throw new KeyNotFoundException($"Book {dto.BookId} not found.");

        // if book is on loan, it cannot be marked as available
        if (await IsBookOnLoanAsync(dto.BookId, ct) && !dto.IsAvailable) throw new InvalidOperationException("Cannot mark a book as unavailable while it is on loan.");

        book.UpdateFromDto(dto);
        // // Set original RowVersion for concurrency check
        // _db.Entry(book).Property(e => e.RowVersion).OriginalValue = dto.RowVersion;

        await _db.SaveChangesAsync(ct);
    }

    // private method to check if book is on loan
    private async Task<bool> IsBookOnLoanAsync(int bookId, CancellationToken ct = default)
    {
        return await _db.Loans.AnyAsync(l => l.BookId == bookId && l.ReturnDate == null, ct);
    }
}