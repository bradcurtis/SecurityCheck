using EmailSecurityApi.DAL;
using EmailSecurityApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Decide repository type
bool useDatabase = builder.Configuration.GetValue<bool>("RepositorySettings:UseDatabase");

if (useDatabase)
{
    builder.Services.AddDbContext<EmailDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IEmailRepository, SqlEmailRepository>();
}
else
{
    builder.Services.AddScoped<IEmailRepository, MockEmailRepository>();
}

// Add logging wrapper
builder.Services.AddScoped<ApiLogger>();

// Register OpenAPI support
builder.Services.AddOpenApi();

// Configure CORS
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

// ✅ Now you can use `app`
var logger = app.Services.GetRequiredService<ILogger<Program>>();
if (useDatabase)
    logger.LogInformation("We are using the SQL Repository");
else
    logger.LogInformation("We are using the Mock Repository");

// Middleware pipeline
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapOpenApi();
app.MapControllers();

app.Run();