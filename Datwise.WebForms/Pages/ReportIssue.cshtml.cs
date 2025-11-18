using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Datwise.WebForms.Pages
{
    public class ReportIssueModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ReportIssueModel> _logger;
        private readonly IConfiguration _configuration;

        public string? TitleError { get; set; }
        public string? DescriptionError { get; set; }
        public string? SeverityError { get; set; }
        public string? DepartmentError { get; set; }
        public string? LocationError { get; set; }
        public string? ReportedByError { get; set; }
        public string? GeneralError { get; set; }
        public string? SuccessMessage { get; set; }

        public ReportIssueModel(HttpClient httpClient, ILogger<ReportIssueModel> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnPostAsync(
            [FromForm] string title,
            [FromForm] string description,
            [FromForm] string severity,
            [FromForm] string department,
            [FromForm] string location,
            [FromForm] string reportedBy)
        {
            _logger.LogInformation($"OnPostAsync called with Title: {title}, ReportedBy: {reportedBy}");

            // Form Validation
            bool isValid = ValidateForm(title, description, severity, department, location, reportedBy);

            if (!isValid)
            {
                _logger.LogWarning("Form validation failed");
                return Page();
            }

            try
            {
                // Create issue object
                var issue = new
                {
                    title = title.Trim(),
                    description = description.Trim(),
                    severity = severity.Trim(),
                    status = "Open",
                    reportedBy = reportedBy.Trim(),
                    department = department.Trim(),
                    location = location.Trim()
                };

                // Prepare JSON content
                var jsonContent = JsonSerializer.Serialize(issue);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                _logger.LogInformation($"Sending request to API with payload: {jsonContent}");

                // Call API endpoint
                var apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:53486";
                
                // Convert HTTPS to HTTP for development
                if (apiBaseUrl.StartsWith("https://localhost"))
                {
                    apiBaseUrl = apiBaseUrl.Replace("https://", "http://");
                }
                
                var apiEndpoint = $"{apiBaseUrl}/api/issues";
                
                _logger.LogInformation($"API Endpoint: {apiEndpoint}");

                var response = await _httpClient.PostAsync(apiEndpoint, content);

                _logger.LogInformation($"API Response Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    SuccessMessage = "Issue reported successfully! Thank you for your report.";
                    _logger.LogInformation($"New issue reported successfully: {title} by {reportedBy}");
                    // Redirect back to dashboard
                    return Redirect("/");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API error: {response.StatusCode} - {errorContent}");
                    GeneralError = $"Failed to submit the report. API Error: {response.StatusCode}";
                    return Page();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Request error submitting report");
                GeneralError = $"Failed to connect to the service. Please ensure the API is running on http://localhost:53487. Error: {ex.Message}";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error submitting report");
                GeneralError = $"An unexpected error occurred: {ex.Message}";
                return Page();
            }
        }

        private bool ValidateForm(string title, string description, string severity, string department, string location, string reportedBy)
        {
            bool isValid = true;

            // Validate Title
            if (string.IsNullOrWhiteSpace(title))
            {
                TitleError = "Title is required";
                isValid = false;
            }
            else if (title.Length > 255)
            {
                TitleError = "Title cannot exceed 255 characters";
                isValid = false;
            }

            // Validate Description
            if (string.IsNullOrWhiteSpace(description))
            {
                DescriptionError = "Description is required";
                isValid = false;
            }
            else if (description.Length > 2000)
            {
                DescriptionError = "Description cannot exceed 2000 characters";
                isValid = false;
            }

            // Validate Severity
            if (string.IsNullOrWhiteSpace(severity) || !IsValidSeverity(severity))
            {
                SeverityError = "Please select a valid severity level";
                isValid = false;
            }

            // Validate Department
            if (string.IsNullOrWhiteSpace(department))
            {
                DepartmentError = "Department is required";
                isValid = false;
            }
            else if (department.Length > 100)
            {
                DepartmentError = "Department cannot exceed 100 characters";
                isValid = false;
            }

            // Validate Location
            if (string.IsNullOrWhiteSpace(location))
            {
                LocationError = "Location is required";
                isValid = false;
            }
            else if (location.Length > 255)
            {
                LocationError = "Location cannot exceed 255 characters";
                isValid = false;
            }

            // Validate ReportedBy
            if (string.IsNullOrWhiteSpace(reportedBy))
            {
                ReportedByError = "Your name is required";
                isValid = false;
            }
            else if (reportedBy.Length > 255)
            {
                ReportedByError = "Name cannot exceed 255 characters";
                isValid = false;
            }

            return isValid;
        }

        private bool IsValidSeverity(string severity)
        {
            return severity switch
            {
                "Low" or "Medium" or "High" or "Critical" => true,
                _ => false
            };
        }
    }
}
