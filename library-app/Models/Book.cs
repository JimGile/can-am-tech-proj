namespace LibraryApp.Models;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
