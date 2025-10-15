using LibraryApp.Dtos;
using LibraryApp.Services;
using Microsoft.AspNetCore.Authorization;
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

    public async Task<IActionResult> OnGetAsync(int? id, CancellationToken ct)
    {
        if (id == null) return Page();
        var dto = await _bookService.GetByIdAsync(id.Value);
        if (dto == null) return NotFound();
        Book = dto;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(Book.Title))
        {
            ModelState.AddModelError(nameof(Book.Title), "Title is required");
        }

        if (!ModelState.IsValid) return Page();

        if (Book.BookId == 0)
        {
            await _bookService.CreateAsync(Book, ct);
        }
        else
        {
            try
            {
                await _bookService.UpdateAsync(Book, ct);
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                var current = await _bookService.GetByIdAsync(Book.BookId, ct);
                if (current == null)
                {
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit was deleted by another user.");
                    return Page();
                }

                ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another user. The current values are shown — reapply your changes and save again.");
                Book = current;
                return Page();
            }
        }

        return RedirectToPage("./Index");
    }
}
