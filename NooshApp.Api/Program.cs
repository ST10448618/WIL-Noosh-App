using Microsoft.EntityFrameworkCore;
using NooshApp.Api.Data;
using NooshApp.Api.Repositories;
using NooshApp.Api.Repositories.Interfaces;
using NooshApp.Api.Services;
using NooshApp.Api.Services.Interfaces;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

// ---- Firebase init (must come after builder exists, before app.Build()) ----
var firebaseKeyBase64 = builder.Configuration["Firebase:ServiceAccountKeyBase64"];
var firebaseKeyPath = builder.Configuration["Firebase:ServiceAccountPath"];

if (!string.IsNullOrEmpty(firebaseKeyBase64))
{
    var tempPath = Path.Combine(Path.GetTempPath(), "firebase-key.json");
    File.WriteAllBytes(tempPath, Convert.FromBase64String(firebaseKeyBase64));
    FirebaseApp.Create(new AppOptions { Credential = GoogleCredential.FromFile(tempPath) });
}
else if (!string.IsNullOrEmpty(firebaseKeyPath) && File.Exists(firebaseKeyPath))
{
    FirebaseApp.Create(new AppOptions { Credential = GoogleCredential.FromFile(firebaseKeyPath) });
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRewardRuleRepository, RewardRuleRepository>();
builder.Services.AddScoped<IPointsRepository, PointsRepository>();
builder.Services.AddScoped<IScanTokenRepository, ScanTokenRepository>();
builder.Services.AddScoped<IReceiptSubmissionRepository, ReceiptSubmissionRepository>();
builder.Services.AddScoped<IAppSettingsRepository, AppSettingsRepository>();

builder.Services.AddScoped<IRewardsService, RewardsService>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<NooshApp.Api.Auth.FirebaseAuthFilter>();
builder.Services.AddScoped<NooshApp.Api.Auth.StaffPinFilter>();
builder.Services.AddScoped<NooshApp.Api.Auth.AdminKeyFilter>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 25 * 1024 * 1024;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddResponseCaching();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebFrontend", policy =>
        policy.WithOrigins(
            "https://localhost:7000", "http://localhost:5000",
            "https://nooshapp-web.onrender.com"
        ).AllowAnyMethod().AllowAnyHeader());
});

// ---- Everything above builds configuration; app.Build() must come next ----
var app = builder.Build();

// ---- Migrate + seed AFTER app exists, using the same configuration ----
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    var selfBaseUrl = app.Configuration["SelfBaseUrl"] ?? "http://localhost:5050";
}

app.MapGet("/health", () => Results.Ok(new { status = "awake", time = DateTime.UtcNow }));

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowWebFrontend");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseResponseCaching();
app.UseAuthorization();
app.MapControllers();

app.Run();