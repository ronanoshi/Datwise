using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Datwise.Models;
using Datwise.Contracts;

namespace Datwise.Data
{
    public class IssueRepository : IIssueRepository
    {
        private readonly DatwiseDbContext _context;

        public IssueRepository(DatwiseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Issue>> GetOpenIssuesAsync()
        {
            return await GetIssuesAsync(status: "Open");
        }

        public async Task<IEnumerable<Issue>> GetIssuesBySeverityAsync(string severity)
        {
            return await GetIssuesAsync(severity: severity, status: "Open");
        }

        public async Task<IEnumerable<Issue>> GetIssuesByStatusAsync(string status)
        {
            return await GetIssuesAsync(status: status);
        }

        public async Task<Issue?> GetIssueByIdAsync(int id)
        {
            return await _context.Issues.FindAsync(id);
        }

        public async Task<IEnumerable<Issue>> GetAllIssuesAsync()
        {
            return await _context.Issues
                .OrderByDescending(i => i.ReportedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Issue>> GetIssuesAsync(string? status = null, string? severity = null, string? sortBy = null, bool sortDescending = false)
        {
            var query = _context.Issues.AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(i => i.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(severity))
            {
                query = query.Where(i => i.Severity == severity);
            }

            // Apply sorting
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                // Default sorting: by severity priority, then by date
                query = query.OrderByDescending(i => i.Severity == "Critical" ? 0 : i.Severity == "High" ? 1 : i.Severity == "Medium" ? 2 : 3)
                             .ThenByDescending(i => i.ReportedDate);
            }
            else
            {
                query = ApplySorting(query, sortBy, sortDescending);
            }

            return await query.ToListAsync();
        }

        private IQueryable<Issue> ApplySorting(IQueryable<Issue> query, string sortBy, bool descending)
        {
            // Normalize sort field name
            var field = sortBy.TrimStart('-');
            var isDescending = sortBy.StartsWith('-') || descending;

            return field.ToLower() switch
            {
                "id" => isDescending ? query.OrderByDescending(i => i.Id) : query.OrderBy(i => i.Id),
                "title" => isDescending ? query.OrderByDescending(i => i.Title) : query.OrderBy(i => i.Title),
                "severity" => isDescending ? query.OrderByDescending(i => i.Severity) : query.OrderBy(i => i.Severity),
                "status" => isDescending ? query.OrderByDescending(i => i.Status) : query.OrderBy(i => i.Status),
                "department" => isDescending ? query.OrderByDescending(i => i.Department) : query.OrderBy(i => i.Department),
                "location" => isDescending ? query.OrderByDescending(i => i.Location) : query.OrderBy(i => i.Location),
                "reportedby" => isDescending ? query.OrderByDescending(i => i.ReportedBy) : query.OrderBy(i => i.ReportedBy),
                "reporteddate" => isDescending ? query.OrderByDescending(i => i.ReportedDate) : query.OrderBy(i => i.ReportedDate),
                "date" => isDescending ? query.OrderByDescending(i => i.ReportedDate) : query.OrderBy(i => i.ReportedDate),
                _ => query.OrderByDescending(i => i.ReportedDate)
            };
        }

        public async Task<int> CreateIssueAsync(Issue issue)
        {
            issue.ReportedDate = DateTime.UtcNow;
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();
            return issue.Id;
        }

        public async Task<bool> UpdateIssueAsync(Issue issue)
        {
            try
            {
                _context.Issues.Update(issue);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteIssueAsync(int id)
        {
            try
            {
                var issue = await _context.Issues.FindAsync(id);
                if (issue == null)
                    return false;

                _context.Issues.Remove(issue);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IssueStatistics> GetIssueStatisticsAsync()
        {
            var issues = await _context.Issues.ToListAsync();
            var now = DateTime.UtcNow;
            var monthStart = new DateTime(now.Year, now.Month, 1);

            return new IssueStatistics
            {
                TotalOpenIssues = issues.Count(i => i.Status == "Open" || i.Status == "In Progress"),
                CriticalIssuesCount = issues.Count(i => i.Severity == "Critical" && (i.Status == "Open" || i.Status == "In Progress")),
                HighSeverityCount = issues.Count(i => i.Severity == "High" && (i.Status == "Open" || i.Status == "In Progress")),
                MediumSeverityCount = issues.Count(i => i.Severity == "Medium" && (i.Status == "Open" || i.Status == "In Progress")),
                LowSeverityCount = issues.Count(i => i.Severity == "Low" && (i.Status == "Open" || i.Status == "In Progress")),
                LastIssueDate = issues.Any() ? issues.Max(i => i.ReportedDate) : DateTime.MinValue,
                ResolvedThisMonth = issues.Count(i => i.Status == "Resolved" && i.ResolvedDate.HasValue && i.ResolvedDate.Value >= monthStart)
            };
        }
    }
}
