using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure HttpClient for API communication
builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapRazorPages();

app.Run();
