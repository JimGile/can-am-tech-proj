using LibraryApp.Dtos;

namespace LibraryApp.Services;

public interface IMemberService
{
    Task<IEnumerable<MemberDto>> GetAllAsync();
    Task<MemberDto?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves a member by their unique identifier.
    /// </summary>
    /// <param name="memberId">The member's ID.</param>
    /// <returns>The member if found, otherwise null.</returns>
    Task<MemberDto?> GetMemberByIdAsync(int memberId);
    Task<MemberDto> CreateAsync(MemberDto dto);
    Task<bool> UpdateAsync(int id, MemberDto dto);
    Task<bool> DeleteAsync(int id);
}
