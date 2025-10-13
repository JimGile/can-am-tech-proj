using LibraryApp.Dtos;

namespace LibraryApp.Models;

public class Member
{

    public int MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime MembershipDate { get; set; }
    public bool IsActive { get; set; }

    public Member()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        MembershipDate = DateTime.UtcNow;
        IsActive = true;
    }

    public Member(MemberDto dto)
    {
        MemberId = dto.MemberId;
        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Email = dto.Email;
        MembershipDate = dto.MembershipDate == default ? DateTime.UtcNow : dto.MembershipDate;
        IsActive = dto.IsActive;
    }
    
    public void UpdateFromDto(MemberDto dto)
    {
        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Email = dto.Email;
        MembershipDate = dto.MembershipDate;
        IsActive = dto.IsActive;
    }

    public string FullName => $"{FirstName} {LastName}";
}
