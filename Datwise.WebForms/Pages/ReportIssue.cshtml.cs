using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
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

        public string? TitleError { get; set; }
        public string? DescriptionError { get; set; }
        public string? SeverityError { get; set; }
        public string? DepartmentError { get; set; }
        public string? LocationError { get; set; }
        public string? ReportedByError { get; set; }
        public string? GeneralError { get; set; }
        public string? SuccessMessage { get; set; }

        public ReportIssueModel(HttpClient httpClient, ILogger<ReportIssueModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync(
            string title,
            string description,
            string severity,
            string department,
            string location,
            string reportedBy)
        {
            // Form Validation
            bool isValid = ValidateForm(title, description, severity, department, location, reportedBy);

            if (!isValid)
                return Page();

            try
            {
                // Create issue object
                var issue = new
                {
                    title = title.Trim(),
                    description = description.Trim(),
                    severity = severity,
                    status = "Open",
                    reportedBy = reportedBy.Trim(),
                    department = department.Trim(),
                    location = location.Trim()
                };

                // Prepare JSON content
                var jsonContent = JsonSerializer.Serialize(issue);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Call API endpoint
                var apiUrl = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["ApiBaseUrl"] ?? "https://localhost:53486";
                var response = await _httpClient.PostAsync($"{apiUrl}/api/issues", content);

                if (response.IsSuccessStatusCode)
                {
                    SuccessMessage = "Issue reported successfully! Thank you for your report.";
                    _logger.LogInformation($"New issue reported: {title} by {reportedBy}");
                    // Optionally redirect back to dashboard after a delay
                    return Redirect("/");
                }
                else
                {
                    GeneralError = "Failed to submit the report. Please try again.";
                    _logger.LogError($"API error: {response.StatusCode}");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                GeneralError = "An error occurred while submitting the report. Please try again later.";
                _logger.LogError(ex, "Error submitting report");
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
