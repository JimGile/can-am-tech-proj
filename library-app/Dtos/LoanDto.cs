using LibraryApp.Models;

namespace LibraryApp.Dtos;

public class LoanDto
{
    public int LoanId { get; set; }
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }

    public LoanDto()
    {
        LoanDate = DateTime.UtcNow;
        DueDate = LoanDate.AddDays(14);
    }

    public LoanDto(Loan loan)
    {
        LoanId = loan.LoanId;
        BookId = loan.BookId;
        MemberId = loan.MemberId;
        LoanDate = loan.LoanDate;
        ReturnDate = loan.ReturnDate;
        DueDate = loan.DueDate;
    }

    public bool IsValid()
    {
        return BookId > 0 && MemberId > 0;
    }
}
