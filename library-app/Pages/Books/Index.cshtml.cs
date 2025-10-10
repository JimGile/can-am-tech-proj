using LibraryApp.Dtos;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryApp.Pages.Books;

public class IndexModel : PageModel
{
    private readonly IBookService _bookService;
    public IndexModel(IBookService bookService) => _bookService = bookService;

    public IList<BookDto> Books { get; set; } = new List<BookDto>();

    public async Task OnGetAsync()
    {
        Books = (await _bookService.GetAllAsync()).ToList();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _bookService.DeleteAsync(id);
        return RedirectToPage();
    }
}
