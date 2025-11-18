using Datwise.Services;
using Datwise.Data;
using Datwise.Contracts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IExampleService, ExampleService>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();
builder.Services.AddScoped<IIssueService, IssueService>();

// Configure EF Core with SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatwiseDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// Initialize database and seed test data on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatwiseDbContext>();
    try
    {
        // Create database if it doesn't exist
        dbContext.Database.EnsureCreated();
        Console.WriteLine("? Database created/verified successfully");

        // Seed test data
        dbContext.SeedTestData();
        Console.WriteLine("? Test data seeded successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"? Error initializing database: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
