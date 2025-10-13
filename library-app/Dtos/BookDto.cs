using LibraryApp.Models;

namespace LibraryApp.Dtos;

public class BookDto
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int? CategoryId { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime DateCreated { get; set; }

    public BookDto()
    {
        Title = string.Empty;
        Author = string.Empty;
    }

    public BookDto(Book book)
    {
        BookId = book.BookId;
        Title = book.Title;
        Author = book.Author;
        CategoryId = book.CategoryId;
        IsAvailable = book.IsAvailable;
        DateCreated = book.DateCreated;
    }
}
