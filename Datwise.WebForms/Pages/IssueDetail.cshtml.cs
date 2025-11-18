using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Datwise.WebForms.Models;

namespace Datwise.WebForms.Pages
{
    public class IssueDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IssueDetailModel> _logger;
        private readonly IConfiguration _configuration;

        public IssueViewModel? Issue { get; set; }
        public string? ErrorMessage { get; set; }

        public IssueDetailModel(HttpClient httpClient, ILogger<IssueDetailModel> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Loading issue details for ID: {id}");

                var apiUrl = _configuration["ApiBaseUrl"] ?? "http://localhost:53487";
                var endpoint = $"{apiUrl}/api/issues/{id}";

                _logger.LogInformation($"Calling API: {endpoint}");

                var response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Issue = JsonSerializer.Deserialize<IssueViewModel>(content, options);

                    if (Issue != null)
                    {
                        _logger.LogInformation($"Successfully loaded issue: {Issue.Title}");
                        return Page();
                    }
                    else
                    {
                        ErrorMessage = "Issue data could not be parsed.";
                        _logger.LogWarning("Issue data was null after deserialization");
                        return Page();
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ErrorMessage = "Issue not found. It may have been deleted.";
                    _logger.LogWarning($"Issue {id} not found (404)");
                    return Page();
                }
                else
                {
                    ErrorMessage = $"Failed to load issue. API Error: {response.StatusCode}";
                    _logger.LogError($"API error: {response.StatusCode}");
                    return Page();
                }
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = "Failed to connect to the service. Please ensure the API is running.";
                _logger.LogError(ex, "HTTP Request error loading issue detail");
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                _logger.LogError(ex, "Unexpected error loading issue detail");
                return Page();
            }
        }
    }
}
