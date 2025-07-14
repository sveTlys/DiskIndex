using DiskIndex.Components;
using DiskIndex.Models;
using DiskIndex.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// MudBlazor default services
builder.Services.AddMudServices();

// Services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<FileSyncService>();
builder.Services.AddDbContext<FileDbContext>(options =>
    options.UseSqlite("Data Source=FileDatabase.db"));

builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<UserAccount>, PasswordHasher<UserAccount>>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FileDbContext>();
    dbContext.Database.EnsureCreated();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FileDbContext>();

    if (!db.Users.Any(u => u.Username == "Admin"))
    {
        var hasher = new PasswordHasher<UserAccount>();
        var admin = new UserAccount
        {
            Username = "Admin",
            Role = "Administrator",
        };
        admin.PasswordHash = hasher.HashPassword(admin, "admin123");
        db.Users.Add(admin);
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
