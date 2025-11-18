using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Datwise.WebForms.Models;

namespace Datwise.WebForms.Pages;

public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;

    public ControlPanelViewModel ControlPanel { get; set; } = new();
    public string? CurrentSort { get; set; }

    public IndexModel(HttpClient httpClient, ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task OnGetAsync(string? sort)
    {
        CurrentSort = sort;
        try
        {
            // Initialize the control panel data
            await LoadStatisticsAsync();
            await LoadOpenIssuesAsync(sort);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading control panel data");
            ControlPanel.ErrorMessage = "Failed to load control panel data. Please refresh the page.";
        }
    }

    private async Task LoadStatisticsAsync()
    {
        try
        {
            var apiUrl = _configuration["ApiBaseUrl"] ?? "http://localhost:53487";
            var response = await _httpClient.GetAsync($"{apiUrl}/api/issues/statistics/summary");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var stats = JsonSerializer.Deserialize<IssueStatisticsViewModel>(content, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (stats != null)
                {
                    ControlPanel.Statistics = stats;
                }
            }
            else
            {
                _logger.LogWarning($"API call failed: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading statistics");
        }
    }

    private async Task LoadOpenIssuesAsync(string? sort = null)
    {
        try
        {
            var apiUrl = _configuration["ApiBaseUrl"] ?? "http://localhost:53487";
            var endpoint = $"{apiUrl}/api/issues/open";
            
            if (!string.IsNullOrWhiteSpace(sort))
            {
                endpoint += $"?sort={Uri.EscapeDataString(sort)}";
            }

            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var issues = JsonSerializer.Deserialize<List<IssueViewModel>>(content, options);

                if (issues != null)
                {
                    ControlPanel.OpenIssues = issues;
                }
            }
            else
            {
                _logger.LogWarning($"API call failed: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading open issues");
        }
    }
}
