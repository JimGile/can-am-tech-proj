using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryApp.Pages
{
    /// <summary>
    /// Handles member logoff by clearing session.
    /// </summary>
    /// <remarks>
    /// Why: FR-003: Allow members to log out and clear authentication state.
    /// </remarks>
    public class LogoffModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
