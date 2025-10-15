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

        modelBuilder.Entity<Book>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Loan>()
            .HasOne<Book>()
            .WithMany()
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Loan>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(l => l.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Fiction" },
            new Category { CategoryId = 2, Name = "Non-Fiction" }
        );        

        modelBuilder.Entity<Book>().HasData(
            new Book { BookId = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", CategoryId = 1, IsAvailable = true, DateCreated = DateTime.UtcNow },
            new Book { BookId = 2, Title = "1984", Author = "George Orwell", CategoryId = 1, IsAvailable = true, DateCreated = DateTime.UtcNow },
            new Book { BookId = 3, Title = "To Kill a Mockingbird", Author = "Harper Lee", CategoryId = 1, IsAvailable = true, DateCreated = DateTime.UtcNow }
        );

        modelBuilder.Entity<Member>().HasData(
            new Member { MemberId = 1, FirstName = "John", LastName = "Doe", Email = "iBZ9y@example.com", MembershipDate = DateTime.UtcNow, IsActive = true },
            new Member { MemberId = 2, FirstName = "Jane", LastName = "Smith", Email = "bNQ5f@example.com", MembershipDate = DateTime.UtcNow, IsActive = true }
        );
    }
}
