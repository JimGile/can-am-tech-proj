using LibraryApp.Data;
using LibraryApp.Dtos;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class CategoryService : ICategoryService
{
    private readonly LibraryContext _db;
    public CategoryService(LibraryContext db) => _db = db;

    public async Task<CategoryDto> CreateAsync(CategoryDto dto, CancellationToken ct = default)
    {
        var c = new Category { Name = dto.Name, Description = dto.Description };
        _db.Categories.Add(c);
        await _db.SaveChangesAsync();
        dto.CategoryId = c.CategoryId;
        return dto;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var c = await _db.Categories.FindAsync(id);
        if (c == null) return false;
        _db.Categories.Remove(c);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Categories.Select(c => new CategoryDto { CategoryId = c.CategoryId, Name = c.Name, Description = c.Description }).ToListAsync();
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var c = await _db.Categories.FindAsync(id);
        if (c == null) return null;
        return new CategoryDto { CategoryId = c.CategoryId, Name = c.Name, Description = c.Description };
    }

    public async Task<bool> UpdateAsync(int id, CategoryDto dto, CancellationToken ct = default)
    {
        var c = await _db.Categories.FindAsync(id);
        if (c == null) return false;
        c.Name = dto.Name;
        c.Description = dto.Description;
        await _db.SaveChangesAsync();
        return true;
    }
}
