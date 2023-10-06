using Microsoft.EntityFrameworkCore;
using Music_Portal.Models;
using Music_Portal.Repository;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.Name = "Session";

});

builder.Services.AddDbContext<MusicPortalContext>(options => options.UseSqlServer(connection));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepository, MusicPortalRepository>();

var app = builder.Build();

app.UseSession();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
