using LibraryApp.Models;

namespace LibraryApp.Dtos;

public class CategoryDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public CategoryDto()
    {
        Name = string.Empty;
    }

    public CategoryDto(Category category)
    {
        CategoryId = category.CategoryId;
        Name = category.Name;
        Description = category.Description;
    }
}
