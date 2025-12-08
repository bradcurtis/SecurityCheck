using EmailSecurityApi.DAL;
using EmailSecurityApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Default repository registration
builder.Services.AddScoped<IEmailRepository, InMemoryEmailRepository>();

// Read setting to decide repository type
bool useDatabase = builder.Configuration.GetValue<bool>("RepositorySettings:UseDatabase");

if (useDatabase)
{
    // SQL repository
    builder.Services.AddDbContext<EmailDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IEmailRepository, SqlEmailRepository>();
}
else
{
    // In-memory repository
    builder.Services.AddScoped<IEmailRepository, InMemoryEmailRepository>();
}

// Add logging wrapper
builder.Services.AddScoped<ApiLogger>();

// Register OpenAPI support (built into .NET 9)
builder.Services.AddOpenApi();

// Configure CORS to allow calls from your add-in dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAddin", policy =>
    {
        policy.WithOrigins(
            "https://localhost:3000",       // dev server
            "https://outlook.office.com"    // Outlook Web
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Use HTTPS redirection (ensures API is available at https://localhost:5199)
app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

// Expose OpenAPI document
app.MapOpenApi();

// Map controllers
app.MapControllers();

app.Run();