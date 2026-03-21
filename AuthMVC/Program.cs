using AuthMVC.CustomValidations;
using AuthMVC.Models.Authentication;
using AuthMVC.Models.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// DbContext ekleme
var connectionString = builder.Configuration.GetConnectionString("SqlServerConnectionString");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity ekleme
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequiredLength = 5; //En az kaç karakterli olması gerektiğini belirtiyoruz.
    options.Password.RequireNonAlphanumeric = false; //Alfanumerik zorunluluğunu kaldırıyoruz.
    options.Password.RequireLowercase = false; //Küçük harf zorunluluğunu kaldırıyoruz.
    options.Password.RequireUppercase = false; //Büyük harf zorunluluğunu kaldırıyoruz.
    options.Password.RequireDigit = false; //0-9 arası sayısal karakter zorunluluğunu kaldırıyoruz.

    options.User.RequireUniqueEmail = true; //Email adreslerini tekilleştiriyoruz.
    options.User.AllowedUserNameCharacters = "abcçdefghiıjklmnoöpqrsştuüvwxyzABCÇDEFGHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._@+"; //Kullanıcı adında geçerli olan karakterleri belirtiyoruz.

})
    .AddPasswordValidator<CustomPasswordValidation>()
    .AddUserValidator<CustomUserValidation>()
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
