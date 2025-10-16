using LibraryApp.Data;
using LibraryApp.Dtos;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class MemberService : IMemberService
{
    private readonly LibraryContext _db;
    public MemberService(LibraryContext db) => _db = db;

    public async Task<IEnumerable<MemberDto>> GetAllAsync()
    {
        return await _db.Members.Select(m => new MemberDto(m)).ToListAsync();
    }

    public async Task<MemberDto?> GetByIdAsync(int id)
    {
        var m = await _db.Members.FindAsync(id);
        if (m == null) return null;
        return new MemberDto(m);
    }

    public async Task<MemberDto> CreateAsync(MemberDto dto)
    {
        var m = new Member(dto);
        _db.Members.Add(m);
        await _db.SaveChangesAsync();
        dto.MemberId = m.MemberId;
        return dto;
    }

    public async Task<bool> UpdateAsync(int id, MemberDto dto)
    {
        var m = await _db.Members.FindAsync(id);
        if (m == null) return false;
        m.UpdateFromDto(dto);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var m = await _db.Members.FindAsync(id);
        if (m == null) return false;
        _db.Members.Remove(m);
        await _db.SaveChangesAsync();
        return true;
    }

}
