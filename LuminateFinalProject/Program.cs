using LuminateFinalProject.DataAccessLayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var conString = builder.Configuration["ConnectionStrings:Default"];
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(conString));

var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );


app.Run();

