using LibraryApp.Dtos;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryApp.Pages.Books;

public class EditModel : PageModel
{
    private readonly IBookService _bookService;
    public EditModel(IBookService bookService) => _bookService = bookService;

    [BindProperty]
    public BookDto Book { get; set; } = new BookDto();

    public bool IsNew => Book.BookId == 0;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return Page();
        var dto = await _bookService.GetByIdAsync(id.Value);
        if (dto == null) return NotFound();
        Book = dto;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Book.Title))
        {
            ModelState.AddModelError(nameof(Book.Title), "Title is required");
        }

        if (!ModelState.IsValid) return Page();

        if (Book.BookId == 0)
        {
            await _bookService.CreateAsync(Book);
        }
        else
        {
            await _bookService.UpdateAsync(Book.BookId, Book);
        }

        return RedirectToPage("Index");
    }
}
