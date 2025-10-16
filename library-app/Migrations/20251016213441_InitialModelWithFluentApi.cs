using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialModelWithFluentApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    MembershipDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Author = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    LoanId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.LoanId);
                    table.ForeignKey(
                        name: "FK_Loans_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Fiction" },
                    { 2, null, "Non-Fiction" },
                    { 3, null, "Science" },
                    { 4, null, "History" },
                    { 5, null, "Self-Help" },
                    { 6, null, "Religion" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "Email", "FirstName", "IsActive", "LastName", "MembershipDate" },
                values: new object[,]
                {
                    { 1, "iBZ9y@example.com", "John", true, "Doe", new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "bNQ5f@example.com", "Jane", true, "Smith", new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Author", "CategoryId", "DateCreated", "IsAvailable", "Title" },
                values: new object[,]
                {
                    { 1, "F. Scott Fitzgerald", 1, new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "The Great Gatsby" },
                    { 2, "George Orwell", 1, new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "1984" },
                    { 3, "Harper Lee", 1, new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "To Kill a Mockingbird" },
                    { 4, "Yuval Noah Harari", 2, new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "Sapiens" },
                    { 5, "Tara Westover", 2, new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "Educated" },
                    { 6, "Michelle Obama", 2, new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "Becoming" },
                    { 7, "J.D. Salinger", 1, new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "The Catcher in the Rye" },
                    { 8, "Aldous Huxley", 1, new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "Brave New World" },
                    { 9, "Rebecca Skloot", 2, new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "The Immortal Life of Henrietta Lacks" },
                    { 10, "Cormac McCarthy", 1, new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "The Road" },
                    { 11, "Daniel Kahneman", 2, new DateTime(2023, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), true, "Thinking, Fast and Slow" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BookId",
                table: "Loans",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_MemberId",
                table: "Loans",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
