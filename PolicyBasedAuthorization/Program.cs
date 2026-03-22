using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PolicyBasedAuthorization.Models.Context;
using PolicyBasedAuthorization.Models.Entities;
using PolicyBasedAuthorization.Requirements;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PolicyBasedContext>(x => x.UseSqlServer("Server = BKARACA; Database = PolicyBasedDB; TrustServerCertificate = True; integrated security = True"));
builder.Services.AddIdentity<AppUser, AppRole>(x =>
{
    x.Password.RequireDigit = false;
    x.Password.RequireLowercase = false;
    x.Password.RequiredLength = 3;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<PolicyBasedContext>();

builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = new PathString("/user/login");
    x.AccessDeniedPath = new PathString("/user/denied");
    x.SlidingExpiration = true;
    x.ExpireTimeSpan = TimeSpan.FromDays(1);
    x.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = "PolicyBased",
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.Always
    };
});

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("TimeControl", policy => policy.Requirements.Add(new TimeRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, TimeHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
