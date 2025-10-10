using LibraryApp.Dtos;

namespace LibraryApp.Services;

public interface ILoanService
{
    Task<IEnumerable<LoanDto>> GetAllAsync();
    Task<LoanDto?> GetByIdAsync(int id);
    Task<LoanDto> CreateAsync(LoanDto dto);
    Task<bool> UpdateAsync(int id, LoanDto dto);
    Task<bool> DeleteAsync(int id);
}
