using AuthMVC.CustomValidations;
using AuthMVC.Models.Authentication;
using AuthMVC.Models.Contexts;
using Microsoft.AspNetCore.Identity;
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
    .AddErrorDescriber<CustomIdentityErrorDescriber>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Login";
    options.LogoutPath = "/User/Logout";
    options.Cookie.Name = "IdentityCookie"; //Cookie ismi
    options.Cookie.HttpOnly = true; // Client-side erişimini engeller.

    /*None – Cookie'leri tüm 3. taraf isteklere ekler.
    Strict – Cookie'leri hiçbir 3. taraf isteğe göndermez.
    Lax – Cookie'leri yalnızca adres çubuğunu değiştirmeyen isteklere göndermez.*/
    options.Cookie.SameSite = SameSiteMode.Lax;

    /*Always – Cookie'leri yalnızca HTTPS üzerinden erişilebilir yapar.
     SameAsRequest – Cookie'leri hem HTTP hem de HTTPS üzerinden erişilebilir yapar.
     None – Cookie'leri yalnızca HTTP üzerinden erişilebilir yapar.*/
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.SlidingExpiration = true; // Sürenin yarısında istek gelirse, süre sıfırlanır.
    options.ExpireTimeSpan = TimeSpan.FromMinutes(2); // Cookie süresi 2 dakika.
});

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
