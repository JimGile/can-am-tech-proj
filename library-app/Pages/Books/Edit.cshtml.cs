using LibraryApp.Dtos;
using LibraryApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; // Ensure this is present
using Microsoft.EntityFrameworkCore; // For DbUpdateConcurrencyException

namespace LibraryApp.Pages.Books;

public class EditModel : PageModel
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;

    public EditModel(IBookService bookService, ICategoryService categoryService)
    {
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        Book = new BookDto();
        Categories = new SelectList(Enumerable.Empty<CategoryDto>(), nameof(CategoryDto.CategoryId), nameof(CategoryDto.Name));
    }

    [BindProperty(SupportsGet = true)]
    public BookDto Book { get; set; } // Change to public set for direct assignment

    public SelectList Categories { get; set; } // Change to public set for direct assignment

    public bool IsNew => Book?.BookId == 0; // Null-check Book

    public async Task<IActionResult> OnGetAsync(int? id, CancellationToken ct)
    {
        if (!id.HasValue || id.Value == 0)
        {
            Book = new BookDto(); // Initialize for new book creation
        }
        else
        {
            Book = await _bookService.GetByIdAsync(id.Value, ct);
            if (Book == null) return NotFound();
        }
        await LoadCategoriesAsync(ct); // Load categories for dropdown
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        // Re-load categories if ModelState is invalid, so dropdown is populated on re-display
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(ct);
            return Page();
        }

        if (Book.BookId == 0)
        {
            await _bookService.CreateAsync(Book, ct);
        }
        else
        {
            await _bookService.UpdateAsync(Book, ct);
        }
        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
    {
        await _bookService.DeleteAsync(id, ct);
        return RedirectToPage("./Index");
    }

    // Helper to load categories, to avoid code duplication
    private async Task LoadCategoriesAsync(CancellationToken ct)
    {
        var categoryDtos = await _categoryService.GetAllAsync(ct);
        Categories = new SelectList(categoryDtos, nameof(CategoryDto.CategoryId), nameof(CategoryDto.Name), Book?.CategoryId);
    }

}