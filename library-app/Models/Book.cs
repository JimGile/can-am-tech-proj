using LibraryApp.Dtos;

namespace LibraryApp.Models;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int? CategoryId { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime DateCreated { get; set; }

    public Book()
    {
        Title = string.Empty;
        Author = string.Empty;
        DateCreated = DateTime.UtcNow;
        IsAvailable = true;
    }

    public Book(BookDto dto)
    {
        Title = dto.Title;
        Author = dto.Author;
        CategoryId = dto.CategoryId;
        IsAvailable = dto.IsAvailable;
        DateCreated = dto.DateCreated == default ? DateTime.UtcNow : dto.DateCreated;
    }

    public void UpdateFromDto(BookDto dto)
    {
        Title = dto.Title;
        Author = dto.Author;
        CategoryId = dto.CategoryId;
        IsAvailable = dto.IsAvailable;
        // DateCreated is not updated
    }
}
