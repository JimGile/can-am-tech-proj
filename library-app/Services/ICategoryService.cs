using LibraryApp.Dtos;

namespace LibraryApp.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<CategoryDto>> GetPagedAsync(PagedRequest request, CancellationToken ct = default);
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<CategoryDto> CreateAsync(CategoryDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, CategoryDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
