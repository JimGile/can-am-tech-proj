using LibraryApp.Dtos;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryApp.Pages.Books;

public class IndexModel : PageModel
{
    private readonly IBookService _bookService;
    public IndexModel(IBookService bookService) => _bookService = bookService;

    public PagedResult<BookDto> Books { get; set; } = new PagedResult<BookDto>();

    public async Task OnGetAsync(PagedRequest request)
    {
        if (request.PageNumber < 1) request.PageNumber = 1;
        if (request.PageSize < 1) request.PageSize = 10;
        Books = await _bookService.GetPagedAsync(request);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _bookService.DeleteAsync(id);
        return RedirectToPage();
    }
}
