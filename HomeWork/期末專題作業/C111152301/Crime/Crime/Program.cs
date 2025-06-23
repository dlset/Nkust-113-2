using Crime.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CsvService>();
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options => {
        options.LoginPath = "/Account/Login";
    });
builder.Services.AddAuthorization();
var app = builder.Build();

// �פJ CSV�]�ȭ��}�o���q�Ĥ@���^
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    if (!context.UserAccounts.Any(u => u.Username == "admin"))
    {
        context.UserAccounts.Add(new UserAccount
        {
            Username = "admin",
            Password = "admin123",
            Role = "Admin"
        });
        context.SaveChanges();
    }
    // �۰ʶפJ CSV�]�ȷ��Ƭ��š^
    if (!context.CrimeStats.Any())
    {
        var csvService = scope.ServiceProvider.GetRequiredService<CsvService>();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "a04003301-211552849.csv");
        if (File.Exists(path))
        {
            using var fileStream = File.OpenRead(path);
            await csvService.ImportCsvAsync(fileStream);
        }
    }
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CrimeStats}/{action=Index}/{id?}");
app.Run();