using DOTNETMVC.Models.Tables;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with MySQL configuration
builder.Services.AddDbContext<Dotnet1BlogDemoContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Enable Session Middleware (this is important for login session management)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout as needed
    options.Cookie.HttpOnly = true; // Set HttpOnly to true for security
    options.Cookie.IsEssential = true; // Make session cookie essential for the app to function
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// **Ensure this order is correct**: Session comes before routing.
app.UseSession();  // This is where session middleware is used

// Routing should be placed after session middleware.
app.UseRouting();
app.UseAuthorization();

// **Ensure static files are served correctly**: This is to ensure the UI layout is properly displayed.
app.UseStaticFiles(); // This is necessary for CSS, JS, and images to be served

// Map static assets (optional), if you have specific static resources in the app.
app.MapStaticAssets();

// Default route configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets(); // Ensures static assets are part of the default route if needed.

app.Run();