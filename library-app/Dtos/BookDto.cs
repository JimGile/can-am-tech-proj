using LibraryApp.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Dtos;

public class BookDto
{
    public int BookId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    [StringLength(150)]
    public string Author { get; set; }

    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; } // Add this line

    public bool IsAvailable { get; set; }

    public DateTime DateCreated { get; set; }

    public byte[]? RowVersion { get; set; }

    public BookDto()
    {
        Title = string.Empty;
        Author = string.Empty;
        DateCreated = DateTime.UtcNow;
        IsAvailable = true;
        CategoryId = null;
        CategoryName = null; // Initialize the new property
        RowVersion = Array.Empty<byte>();
    }

    public BookDto(Book book)
    {
        BookId = book.BookId;
        Title = book.Title;
        Author = book.Author;
        CategoryId = book.CategoryId;
        CategoryName = book.Category?.Name; // Populate CategoryName from navigation property
        IsAvailable = book.IsAvailable;
        DateCreated = book.DateCreated;
        RowVersion = book.RowVersion;
    }
}