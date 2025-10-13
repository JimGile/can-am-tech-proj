using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryApp.Services;
using LibraryApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Pages
{
    /// <summary>
    /// Handles member login by validating MemberId and LastName.
    /// </summary>
    /// <remarks>
    /// Why: FR-002: Allow members to login using their MemberId and LastName for basic authentication.
    /// </remarks>
    public class LoginModel : PageModel
    {
        private readonly IMemberService _memberService;
        public LoginModel(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            public int MemberId { get; set; }
            public string LastName { get; set; } = string.Empty;
        }

        public void OnGet()
        {
            // No initialization required for GET. Login form is shown.
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Invalid input.";
                return Page();
            }

            var member = await _memberService.GetMemberByIdAsync(Input.MemberId);
            if (member == null || !string.Equals(member.LastName, Input.LastName, System.StringComparison.OrdinalIgnoreCase))
            {
                ErrorMessage = "Invalid Member ID or Last Name.";
                return Page();
            }

            // Store MemberId and FullName in session
            HttpContext.Session.SetInt32("MemberId", member.MemberId);
            HttpContext.Session.SetString("FullName", member.FirstName + " " + member.LastName);

            return RedirectToPage("/Index");
        }
    }
}
