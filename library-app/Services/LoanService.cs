using LibraryApp.Data;
using LibraryApp.Dtos;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class LoanService : ILoanService
{
    private readonly LibraryContext _db;
    public LoanService(LibraryContext db) => _db = db;

    public async Task<LoanDto> CreateAsync(LoanDto dto)
    {
        // validate ids
        if (dto.BookId <= 0 || dto.MemberId <= 0) return null;

        // load book and member for validation
        var book = await _db.Books.FindAsync(dto.BookId);
        if (book == null || !book.IsAvailable) return null; // book must exist and be available

        var member = await _db.Members.FindAsync(dto.MemberId);
        if (member == null) return null;
        // assume Member has an IsActive flag
        if (member is { } && member.GetType().GetProperty("IsActive") != null)
        {
            // use dynamic property access to avoid compile errors if property name differs
            var isActiveProp = member.GetType().GetProperty("IsActive");
            if (isActiveProp != null)
            {
                var isActive = isActiveProp.GetValue(member) as bool?;
                if (isActive == false) return null;
            }
        }

        // normalize loan date
        var loanDate = dto.LoanDate == default ? DateTime.UtcNow : dto.LoanDate;

        // validate date ordering: loanDate < dueDate and dueDate < returnDate (if returnDate provided)
        if (dto.DueDate == default || !(loanDate < dto.DueDate)) return null;
        if (dto.ReturnDate != null && !(dto.DueDate < dto.ReturnDate)) return null;

        var loan = new Loan
        {
            BookId = dto.BookId,
            MemberId = dto.MemberId,
            LoanDate = loanDate,
            DueDate = dto.DueDate,
            ReturnDate = dto.ReturnDate
        };

        _db.Loans.Add(loan);
        // mark book unavailable (we already loaded it)
        book.IsAvailable = false;
        await _db.SaveChangesAsync();
        dto.LoanId = loan.LoanId;
        return dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var l = await _db.Loans.FindAsync(id);
        if (l == null) return false;
        _db.Loans.Remove(l);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<LoanDto>> GetAllAsync()
    {
        return await _db.Loans.Select(l => new LoanDto
        {
            LoanId = l.LoanId,
            BookId = l.BookId,
            MemberId = l.MemberId,
            LoanDate = l.LoanDate,
            ReturnDate = l.ReturnDate,
            DueDate = l.DueDate
        }).ToListAsync();
    }

    public async Task<LoanDto?> GetByIdAsync(int id)
    {
        var l = await _db.Loans.FindAsync(id);
        if (l == null) return null;
        return new LoanDto
        {
            LoanId = l.LoanId,
            BookId = l.BookId,
            MemberId = l.MemberId,
            LoanDate = l.LoanDate,
            ReturnDate = l.ReturnDate,
            DueDate = l.DueDate
        };
    }

    public async Task<bool> UpdateAsync(int id, LoanDto dto)
    {
        var l = await _db.Loans.FindAsync(id);
        if (l == null) return false;
        l.BookId = dto.BookId;
        l.MemberId = dto.MemberId;
        l.LoanDate = dto.LoanDate;
        l.DueDate = dto.DueDate;
        l.ReturnDate = dto.ReturnDate;
        // if returned, mark book available
        if (dto.ReturnDate != null)
        {
            var book = await _db.Books.FindAsync(dto.BookId);
            if (book != null) book.IsAvailable = true;
        }
        await _db.SaveChangesAsync();
        return true;
    }
}
