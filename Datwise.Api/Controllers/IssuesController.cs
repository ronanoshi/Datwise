using Microsoft.AspNetCore.Mvc;
using Datwise.Services;
using Datwise.Models;
using Datwise.Contracts;

namespace Datwise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueService _issueService;
        private readonly ILogger<IssuesController> _logger;

        public IssuesController(IIssueService issueService, ILogger<IssuesController> logger)
        {
            _issueService = issueService;
            _logger = logger;
        }

        /// <summary>
        /// Get issues with optional filtering and sorting
        /// Supports multiple values for status and severity (comma-separated)
        /// </summary>
        /// <param name="status">Filter by status (comma-separated: Open, In Progress, Resolved)</param>
        /// <param name="severity">Filter by severity (comma-separated: Low, Medium, High, Critical)</param>
        /// <param name="sort">Sort field: id, title, severity, status, department, location, reportedby, date (prefix with - for descending)</param>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issue>>> GetIssues([FromQuery] string? status, [FromQuery] string? severity, [FromQuery] string? sort)
        {
            try
            {
                // Parse sort parameter
                var sortDescending = false;
                var sortBy = sort;
                if (!string.IsNullOrWhiteSpace(sort) && sort.StartsWith('-'))
                {
                    sortBy = sort.Substring(1);
                    sortDescending = true;
                }

                var issues = await _issueService.GetIssuesAsync(status, severity, sortBy, sortDescending);
                return Ok(issues);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving issues");
                return StatusCode(500, new { message = "An error occurred while retrieving issues" });
            }
        }

        /// <summary>
        /// Get issue by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> GetIssueById(int id)
        {
            try
            {
                var issue = await _issueService.GetIssueByIdAsync(id);
                if (issue == null)
                    return NotFound(new { message = "Issue not found" });

                return Ok(issue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving issue by ID");
                return StatusCode(500, new { message = "An error occurred while retrieving the issue" });
            }
        }

        /// <summary>
        /// Get issue statistics
        /// </summary>
        [HttpGet("statistics/summary")]
        public async Task<ActionResult<IssueStatistics>> GetStatistics()
        {
            try
            {
                var stats = await _issueService.GetIssueStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving statistics");
                return StatusCode(500, new { message = "An error occurred while retrieving statistics" });
            }
        }

        /// <summary>
        /// Create a new issue/incident report
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> CreateIssue([FromBody] Issue issue)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var issueId = await _issueService.CreateIssueAsync(issue);
                return CreatedAtAction(nameof(GetIssueById), new { id = issueId }, issueId);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error creating issue");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating issue");
                return StatusCode(500, new { message = "An error occurred while creating the issue report" });
            }
        }

        /// <summary>
        /// Update an existing issue
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateIssue(int id, [FromBody] Issue issue)
        {
            try
            {
                if (id != issue.Id)
                    return BadRequest(new { message = "ID mismatch" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _issueService.UpdateIssueAsync(issue);
                if (!success)
                    return NotFound(new { message = "Issue not found" });

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error updating issue");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating issue");
                return StatusCode(500, new { message = "An error occurred while updating the issue" });
            }
        }

        /// <summary>
        /// Delete an issue
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIssue(int id)
        {
            try
            {
                var success = await _issueService.DeleteIssueAsync(id);
                if (!success)
                    return NotFound(new { message = "Issue not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting issue");
                return StatusCode(500, new { message = "An error occurred while deleting the issue" });
            }
        }
    }
}
