using LibraryApp.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Services;

public interface IBookService
{
    Task<BookDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<BookDto> CreateAsync(BookDto dto, CancellationToken ct = default);
    Task UpdateAsync(BookDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);

    Task<PagedResult<BookDto>> GetPagedAsync(PagedRequest request, CancellationToken ct = default);
}
