using LibraryApp.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Services;

/// <summary>
/// Interface for book services.
/// </summary>
public interface IBookService
{
    /// <summary>
    /// Retrieves a paged list of books.
    /// </summary>
    /// <param name="request">The paged request.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A paged result containing the list of books and the total count.</returns>
    Task<PagedResult<BookDto>> GetPagedAsync(PagedRequest request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a paged list of books by category id.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    /// <param name="request">The paged request.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A paged result containing the list of books and the total count.</returns>
    Task<PagedResult<BookDto>> GetPagedByCategoryIdAsync(int categoryId, PagedRequest request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a book by its identifier.
    /// </summary>
    /// <param name="id">The book identifier.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The book dto if found, otherwise null.</returns>
    Task<BookDto> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new book from the given dto.
    /// </summary>
    /// <param name="dto">The book dto.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The created book dto.</returns>
    Task<BookDto> CreateAsync(BookDto dto, CancellationToken ct = default);

    /// <summary>
    /// Updates a book by its identifier.
    /// </summary>
    /// <param name="dto">The book dto.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>true if the book is found and updated, otherwise false.</returns>
    Task UpdateAsync(BookDto dto, CancellationToken ct = default);

    /// <summary>
    /// Deletes a book by its identifier.
    /// </summary>
    /// <param name="id">The book identifier.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>true if the book is found and deleted, otherwise false.</returns>
    Task DeleteAsync(int id, CancellationToken ct = default);
}