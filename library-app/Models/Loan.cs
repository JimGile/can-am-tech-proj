using LibraryApp.Dtos;

namespace LibraryApp.Models;

public class Loan
{
    public int LoanId { get; set; }
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }

    public Loan()
    {
        LoanDate = DateTime.UtcNow;
        DueDate = LoanDate.AddDays(14);
    }

    public Loan(LoanDto dto)
    {
        BookId = dto.BookId;
        MemberId = dto.MemberId;
        LoanDate = dto.LoanDate;
        ReturnDate = dto.ReturnDate;
        DueDate = dto.DueDate;
    }

    public void UpdateFromDto(LoanDto dto)
    {
        BookId = dto.BookId;
        MemberId = dto.MemberId;
        LoanDate = dto.LoanDate;
        ReturnDate = dto.ReturnDate;
        DueDate = dto.DueDate;
    }
}
