global using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SQLite DbContext
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=library.db"));

// Register services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ILoanService, LoanService>();

// // Configure authentication + authorization
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.LoginPath = "/Login";        // page to redirect unauthenticated users
//         options.AccessDeniedPath = "/";
//         options.Cookie.Name = "LibraryApp.Auth";
//         options.Cookie.HttpOnly = true;
//         options.Cookie.SameSite = SameSiteMode.Lax;  // adjust for your frontend needs
//         options.ExpireTimeSpan = TimeSpan.FromHours(8);
//         // In production consider sliding expiration, secure cookie, and tighter SameSite
//     });

// builder.Services.AddAuthorization(options =>
// {
//     // Add custom policies if needed
//     options.FallbackPolicy = new AuthorizationPolicyBuilder()
//         .RequireAuthenticatedUser()
//         .Build();
// });

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    await db.Database.EnsureCreatedAsync();
}

await app.RunAsync();
