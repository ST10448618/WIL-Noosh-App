using NooshApp.Web.Services;
using NooshApp.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IMenuApiClient, MenuApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5021/");
});

builder.Services.AddHttpClient<ICateringApiClient, CateringApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5021/");
});

builder.Services.AddHttpClient<ICareersApiClient, CareersApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5021/");
});

builder.Services.AddHttpClient<IRewardsApiClient, RewardsApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5021/");
});

builder.Services.AddHttpClient<IStaffApiClient, StaffApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5021/");
});

builder.Services.AddHttpClient<IAdminApiClient, AdminApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5021/");
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
});
builder.Services.AddHttpClient<IFavouritesApiClient, FavouritesApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7050/");
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);   // stays logged in for 30 days
        options.SlidingExpiration = true;                 // refreshes on activity
        options.Cookie.HttpOnly = true;
    });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<DataProtectionKeysContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("KeysConnection")
        ?? "Data Source=DataProtectionKeys.db"));

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<DataProtectionKeysContext>()
    .SetApplicationName("NooshApp"); 

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var keysDb = scope.ServiceProvider.GetRequiredService<DataProtectionKeysContext>();
    keysDb.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/NotFound");
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();