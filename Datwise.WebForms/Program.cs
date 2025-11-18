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

// Get API URL from configuration
var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
Console.WriteLine($"[DEBUG] ApiBaseUrl from config: {apiBaseUrl}");

if (string.IsNullOrEmpty(apiBaseUrl))
{
    apiBaseUrl = "http://localhost:53487";
    Console.WriteLine($"[DEBUG] ApiBaseUrl was null, using default: {apiBaseUrl}");
}

// Configure HttpClient for API communication with explicit BaseAddress
builder.Services.AddHttpClient<ReportIssueModel>()
    .ConfigureHttpClient(client =>
    {
        Console.WriteLine($"[DEBUG] Setting HttpClient BaseAddress to: {apiBaseUrl}");
        client.BaseAddress = new Uri(apiBaseUrl);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.Timeout = TimeSpan.FromSeconds(30);
        Console.WriteLine($"[DEBUG] HttpClient BaseAddress set to: {client.BaseAddress}");
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
