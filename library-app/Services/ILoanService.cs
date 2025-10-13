using LibraryApp.Dtos;
using LibraryApp.Models;

namespace LibraryApp.Services;

public interface ILoanService
{
    Task<IEnumerable<LoanDto>> GetAllAsync(int pageSize, int pageNumber);
    Task<LoanDto?> GetByIdAsync(int id);
    Task<IEnumerable<LoanDto>> GetAllByMemberIdAsync(int memberId, int pageSize, int pageNumber);
    Task<IEnumerable<LoanDto>> GetAllByBookIdAsync(int bookId, int pageSize, int pageNumber);
    Task<IEnumerable<LoanDto>> GetActiveLoansAsync(int pageSize, int pageNumber);
    Task<IEnumerable<LoanDto>> GetActiveLoansByMemberIdAsync(int memberId, int pageSize, int pageNumber);
    Task<LoanDto?> GetActiveLoanByBookIdAsync(int bookId);
    Task<LoanDto> CreateAsync(LoanDto dto);
    Task<bool> UpdateAsync(int id, LoanDto dto);
    Task<bool> DeleteAsync(int id);
}
