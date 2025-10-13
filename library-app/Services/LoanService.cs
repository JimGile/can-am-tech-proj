using LibraryApp.Data;
using LibraryApp.Dtos;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class LoanService : ILoanService
{
    private readonly LibraryContext _db;
    public LoanService(LibraryContext db) => _db = db;

    /// <summary>
    /// Retrieves a list of all loans with pagination and default ordering by LoanDate descending.
    /// </summary>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <returns>A list of all loans.</returns>
    public async Task<IEnumerable<LoanDto>> GetAllAsync(int pageSize, int pageNumber)
    {
        return await _db.Loans
            .OrderByDescending(l => l.LoanDate)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(l => new LoanDto(l)).ToListAsync();
    }

    /// <summary>
    /// Retrieves all loans for a specific member.
    /// </summary>
    /// <param name="memberId">The member identifier.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <returns>A list of all loans for the member, ordered by LoanDate descending.</returns>
    public async Task<IEnumerable<LoanDto>> GetAllByMemberIdAsync(int memberId, int pageSize, int pageNumber)
    {
        return await _db.Loans
            .Where(l => l.MemberId == memberId)
            .OrderByDescending(l => l.LoanDate)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(l => new LoanDto(l)).ToListAsync();
    }

    /// <summary>
    /// Retrieves all loans for a specific book.
    /// </summary>
    /// <param name="bookId">The book identifier.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <returns>A list of all loans for the book, ordered by LoanDate descending.</returns>
    public async Task<IEnumerable<LoanDto>> GetAllByBookIdAsync(int bookId, int pageSize, int pageNumber)
    {
        return await _db.Loans
            .Where(l => l.BookId == bookId)
            .OrderByDescending(l => l.LoanDate)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(l => new LoanDto(l)).ToListAsync();
    }

    /// <summary>
    /// Retrieves all active loans (i.e. loans with a null return date),
    /// ordered by due date ascending.
    /// </summary>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <returns>A list of all active loans for the given page, ordered by due date ascending.</returns>
    public async Task<IEnumerable<LoanDto>> GetActiveLoansAsync(int pageSize, int pageNumber)
    {
        return await _db.Loans
            .Where(l => l.ReturnDate == null)
            .OrderBy(l => l.DueDate)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(l => new LoanDto(l)).ToListAsync();
    }

    // get active loans by member id
    public async Task<IEnumerable<LoanDto>> GetActiveLoansByMemberIdAsync(int memberId, int pageSize, int pageNumber)
    {
        return await _db.Loans
            .Where(l => l.MemberId == memberId && l.ReturnDate == null)
            .OrderBy(l => l.DueDate)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(l => new LoanDto(l)).ToListAsync();
    }

    // get active loan by book id
    public async Task<LoanDto?> GetActiveLoanByBookIdAsync(int bookId)
    {
        var l = await _db.Loans
            .Where(l => l.BookId == bookId && l.ReturnDate == null)
            .OrderBy(l => l.DueDate)
            .FirstOrDefaultAsync();
        if (l == null) return null;
        return new LoanDto(l);
    }

    /// <summary>
    /// Retrieves a specific loan by its identifier.
    /// </summary>
    /// <param name="id">The loan identifier.</param>
    /// <returns>The loan if found, otherwise null.</returns>
    public async Task<LoanDto?> GetByIdAsync(int id)
    {
        var l = await _db.Loans.FindAsync(id);
        if (l == null) return null;
        return new LoanDto(l);
    }

    /// <summary>
    /// Creates a new loan from the given dto.
    /// </summary>
    /// <param name="dto">The loan dto.</param>
    /// <returns>The created loan dto.</returns>
    /// <exception cref="ArgumentException">BookId and MemberId must be valid.</exception>
    /// <exception cref="ArgumentException">Book is not available.</exception>
    /// <exception cref="ArgumentException">Member not found or inactive.</exception>
    public async Task<LoanDto> CreateAsync(LoanDto dto)
    {
        var book = await ValidateForCreateAsync(dto);
        var loan = new Loan(dto);
        _db.Loans.Add(loan);

        // mark book unavailable and save all changes
        book.IsAvailable = false;
        await _db.SaveChangesAsync();

        dto.LoanId = loan.LoanId;
        return dto;
    }

    /// <summary>
    /// Deletes a loan by its identifier.
    /// </summary>
    /// <param name="id">The loan identifier.</param>
    /// <returns>true if the loan is found and deleted, otherwise false.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var l = await _db.Loans.FindAsync(id);
        if (l == null) return false;
        // mark book available
        var book = await _db.Books.FindAsync(l.BookId);
        if (book != null) book.IsAvailable = true;
        _db.Loans.Remove(l);
        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Updates a loan by its identifier.
    /// </summary>
    /// <param name="id">The loan identifier.</param>
    /// <param name="dto">The loan dto.</param>
    /// <returns>true if the loan is found and updated, otherwise false.</returns>
    /// <remarks>
    /// If the loan is updated and the returned date is not null, the book is marked as available.
    /// </remarks>
    public async Task<bool> UpdateAsync(int id, LoanDto dto)
    {
        var l = await _db.Loans.FindAsync(id);
        if (l == null) return false;
        if (l.BookId == dto.BookId)
        {
            // if returned, mark book available
            if (dto.ReturnDate != null && l.ReturnDate == null)
            {
                var book = await _db.Books.FindAsync(l.BookId);
                if (book != null) book.IsAvailable = true;
            }
        }
        else
        {
            // book changed, mark old book available and new book unavailable
            var oldBook = await _db.Books.FindAsync(l.BookId);
            if (oldBook != null) oldBook.IsAvailable = true;
            var newBook = await _db.Books.FindAsync(dto.BookId);
            if (newBook != null) newBook.IsAvailable = false;
        }
        l.UpdateFromDto(dto);
        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Validates the given loan dto, ensuring the book is available and the member is found and active.
    /// </summary>
    /// <param name="dto">The loan dto.</param>
    /// <exception cref="ArgumentException">BookId and MemberId must be valid.</exception>
    /// <exception cref="ArgumentException">Book is not available.</exception>
    /// <exception cref="ArgumentException">Member not found or inactive.</exception>
    private async Task<Book> ValidateForCreateAsync(LoanDto dto)
    {
        // validate ids
        if (!dto.IsValid())
        {
            throw new ArgumentException("BookId and MemberId must be valid");
        }

        // load book and member for validation
        var book = await _db.Books.FindAsync(dto.BookId);
        if (book == null || !book.IsAvailable)
        {
            throw new ArgumentException("Book is not available");
        }

        await ValidateMemberElibilityAsync(dto);
        return book;
    }

    private async Task<bool> ValidateMemberElibilityAsync(LoanDto dto)
    {
        var member = await _db.Members.FindAsync(dto.MemberId);
        if (member != null && member.IsActive)
        {
            var numberOfActiveLoans = await CountNumberOfActiveLoansByMemberAsync(dto);
            if (numberOfActiveLoans >= 3)
            {
                throw new ArgumentException("Member has too many active loans");
            }
        }
        else
        {
            throw new ArgumentException("Member not found or inactive");
        }
        return true;
    }

    private async Task<int> CountNumberOfActiveLoansByMemberAsync(LoanDto dto)
    {
        return await _db.Loans.CountAsync(l => l.MemberId == dto.MemberId && l.ReturnDate == null);
    }

}
