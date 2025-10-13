using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public int? MemberId { get; private set; }
    public string? FullName { get; private set; }

    public void OnGet()
    {
        MemberId = HttpContext.Session.GetInt32("MemberId");
        FullName = HttpContext.Session.GetString("FullName");
    }
}
