using LibraryApp.Models;

namespace LibraryApp.Dtos;

public class MemberDto
{
    public int MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime MembershipDate { get; set; }
    public bool IsActive { get; set; }

    public MemberDto() {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        MembershipDate = DateTime.UtcNow;
        IsActive = true;
    }

    public MemberDto(Member member)
    {
        MemberId = member.MemberId;
        FirstName = member.FirstName;
        LastName = member.LastName;
        Email = member.Email;
        MembershipDate = member.MembershipDate;
        IsActive = member.IsActive;
    }

    public string FullName => $"{FirstName} {LastName}";
}
