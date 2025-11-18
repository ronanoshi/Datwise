using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Datwise.WebForms.Pages;
using System;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure HttpClient for API communication with proper settings
builder.Services.AddHttpClient<ReportIssueModel>(client =>
{
    var apiUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:53486";
    
    // In development, use HTTP instead of HTTPS to avoid certificate issues
    if (builder.Environment.IsDevelopment())
    {
        apiUrl = apiUrl.Replace("https://", "http://");
    }
    
    client.BaseAddress = new Uri(apiUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigureHttpClient(client =>
{
    // Configure client behavior
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Also add general HttpClient for other pages
builder.Services.AddHttpClient();

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
