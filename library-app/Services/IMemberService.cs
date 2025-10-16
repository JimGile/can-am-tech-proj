using LibraryApp.Dtos;

namespace LibraryApp.Services;

public interface IMemberService
{
    Task<IEnumerable<MemberDto>> GetAllAsync();
    Task<MemberDto?> GetByIdAsync(int id);
    Task<MemberDto> CreateAsync(MemberDto dto);
    Task<bool> UpdateAsync(int id, MemberDto dto);
    Task<bool> DeleteAsync(int id);
}
