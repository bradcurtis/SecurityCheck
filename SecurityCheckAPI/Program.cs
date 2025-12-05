using EmailSecurityApi.DAL;
using EmailSecurityApi.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddScoped<IEmailRepository, MockEmailRepository>();
builder.Services.AddScoped<IEmailRepository, InMemoryEmailRepository>();

// Read setting
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


builder.Services.AddScoped<ApiLogger>();

// Register OpenAPI support (built into .NET 9)
builder.Services.AddOpenApi();

var app = builder.Build();

// Expose OpenAPI document
app.MapOpenApi();

// Map controllers
app.MapControllers();

app.Run();