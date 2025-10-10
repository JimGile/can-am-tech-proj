using LibraryApp.Dtos;

namespace LibraryApp.Services;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllAsync();
    Task<BookDto?> GetByIdAsync(int id);
    Task<BookDto> CreateAsync(BookDto dto);
    Task<bool> UpdateAsync(int id, BookDto dto);
    Task<bool> DeleteAsync(int id);
}
