namespace LibraryApp.Dtos;

public class BookDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime DateCreated { get; set; }
}
