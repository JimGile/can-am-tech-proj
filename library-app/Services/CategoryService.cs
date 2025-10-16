using LibraryApp.Data;
using LibraryApp.Dtos;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class CategoryService : ICategoryService
{
    private readonly LibraryContext _db;

    public CategoryService(LibraryContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Categories.Select(c => new CategoryDto { CategoryId = c.CategoryId, Name = c.Name, Description = c.Description }).ToListAsync(ct);
    }

    public async Task<PagedResult<CategoryDto>> GetPagedAsync(PagedRequest request, CancellationToken ct = default)
    {
        var query = _db.Categories.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(c => c.Name.Contains(s));
        }

        bool desc = string.Equals(request.SortDir, "desc", StringComparison.OrdinalIgnoreCase);
        query = request.SortBy?.ToLowerInvariant() switch
        {
            "name" => desc ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
            _ => desc ? query.OrderByDescending(c => c.CategoryId) : query.OrderBy(c => c.CategoryId),
        };

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CategoryDto { CategoryId = c.CategoryId, Name = c.Name, Description = c.Description })
            .ToListAsync(ct);

        return new PagedResult<CategoryDto>(items.AsReadOnly(), total, request);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var c = await _db.Categories.FindAsync(id, ct);
        if (c == null) return null;
        return new CategoryDto { CategoryId = c.CategoryId, Name = c.Name, Description = c.Description };
    }

    public async Task<CategoryDto> CreateAsync(CategoryDto dto, CancellationToken ct = default)
    {
        var c = new Category { Name = dto.Name, Description = dto.Description };
        _db.Categories.Add(c);
        await _db.SaveChangesAsync(ct);
        dto.CategoryId = c.CategoryId;
        return dto;
    }

    public async Task<bool> UpdateAsync(int id, CategoryDto dto, CancellationToken ct = default)
    {
        var c = await _db.Categories.FindAsync(id);
        if (c == null) return false;
        c.Name = dto.Name;
        c.Description = dto.Description;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var c = await _db.Categories.FindAsync(id);
        if (c == null) return false;
        _db.Categories.Remove(c);
        await _db.SaveChangesAsync(ct);
        return true;
    }

}
