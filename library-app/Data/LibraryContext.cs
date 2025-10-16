using LibraryApp.Models;

namespace LibraryApp.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Loan> Loans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>().HasKey(b => b.BookId);
        modelBuilder.Entity<Member>().HasKey(m => m.MemberId);
        modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
        modelBuilder.Entity<Loan>().HasKey(l => l.LoanId);

        // Configure the relationship between Book and Category
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Category) // A Book has one Category
            .WithMany() // A Category can have many Books (no navigation property on Category side explicitly defined in Category.cs)
            .HasForeignKey(b => b.CategoryId) // The foreign key is Book.CategoryId
            .IsRequired(false) // CategoryId is nullable, so the relationship is optional
            .OnDelete(DeleteBehavior.SetNull); // When a Category is deleted, set Book.CategoryId to NULL
                                                // (if CategoryId was not nullable, this would be DeleteBehavior.Cascade)
        
        // Configure the relationships for Loan
        modelBuilder.Entity<Loan>()
            .HasOne<Book>()
            .WithMany()
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the relationships for Loan
        modelBuilder.Entity<Loan>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(l => l.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Fiction" },
            new Category { CategoryId = 2, Name = "Non-Fiction" },
            new Category { CategoryId = 3, Name = "Science" },
            new Category { CategoryId = 4, Name = "History" },
            new Category { CategoryId = 5, Name = "Self-Help" },
            new Category { CategoryId = 6, Name = "Religion" }
        );        

        modelBuilder.Entity<Book>().HasData(
            new Book { BookId = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", CategoryId = 1, IsAvailable = true, DateCreated = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc) },
            new Book { BookId = 2, Title = "1984", Author = "George Orwell", CategoryId = 1, IsAvailable = true, DateCreated = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc) },
            new Book { BookId = 3, Title = "To Kill a Mockingbird", Author = "Harper Lee", CategoryId = 1, IsAvailable = true, DateCreated = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc) },
            new Book { BookId = 4, Title = "Sapiens", Author = "Yuval Noah Harari", CategoryId = 2, IsAvailable = true, DateCreated = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc) },
            new Book { BookId = 5, Title = "Educated", Author = "Tara Westover", CategoryId = 2, IsAvailable = true, DateCreated = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc) },
            new Book { BookId = 6, Title = "Becoming", Author = "Michelle Obama", CategoryId = 2, IsAvailable = true, DateCreated = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc) },
            new Book { BookId = 7, Title = "The Catcher in the Rye", Author = "J.D. Salinger", CategoryId = 1, IsAvailable = true, DateCreated = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc) },
            new Book { BookId = 8, Title = "Brave New World", Author = "Aldous Huxley", CategoryId = 1, IsAvailable = true, DateCreated = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc) },
            new Book { BookId = 9, Title = "The Immortal Life of Henrietta Lacks", Author = "Rebecca Skloot", CategoryId = 2, IsAvailable = true, DateCreated = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc) },
            new Book { BookId = 10, Title = "The Road", Author = "Cormac McCarthy", CategoryId = 1, IsAvailable = true, DateCreated = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc) },
            new Book { BookId = 11, Title = "Thinking, Fast and Slow", Author = "Daniel Kahneman", CategoryId = 2, IsAvailable = true, DateCreated = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Member>().HasData(
            new Member { MemberId = 1, FirstName = "John", LastName = "Doe", Email = "iBZ9y@example.com", MembershipDate = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc), IsActive = true },
            new Member { MemberId = 2, FirstName = "Jane", LastName = "Smith", Email = "bNQ5f@example.com", MembershipDate = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc), IsActive = true }
        );
    }
}
