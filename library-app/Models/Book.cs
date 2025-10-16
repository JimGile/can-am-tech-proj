using LibraryApp.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models;

public class Book
{
    public int BookId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    [StringLength(150)]
    public string Author { get; set; }

    // This is your explicit foreign key property
    public int? CategoryId { get; set; }

    // This is the display property.
    public Category? Category { get; set; }

    public bool IsAvailable { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime DateCreated { get; set; }

    // optimistic concurrency token
    [Timestamp]
    public byte[]? RowVersion { get; set; }

    public Book()
    {
        Title = string.Empty;
        Author = string.Empty;
        DateCreated = DateTime.UtcNow;
        IsAvailable = true;
        CategoryId = null;
        RowVersion = Array.Empty<byte>();
    }

    public Book(BookDto dto)
    {
        Title = dto.Title;
        Author = dto.Author;
        CategoryId = dto.CategoryId;
        IsAvailable = dto.IsAvailable;
        DateCreated = dto.DateCreated;
        RowVersion = dto.RowVersion;
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