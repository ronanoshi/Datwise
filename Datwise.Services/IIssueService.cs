using System.Collections.Generic;
using System.Threading.Tasks;
using Datwise.Contracts;
using Datwise.Models;

namespace Datwise.Services
{
    public interface IIssueService
    {
        Task<IEnumerable<Issue>> GetOpenIssuesAsync();
        Task<IEnumerable<Issue>> GetIssuesBySeverityAsync(string severity);
        Task<Issue?> GetIssueByIdAsync(int id);
        Task<IEnumerable<Issue>> GetIssuesAsync(string? status = null, string? severity = null, string? sortBy = null, bool sortDescending = false);
        Task<int> CreateIssueAsync(Issue issue);
        Task<bool> UpdateIssueAsync(Issue issue);
        Task<bool> DeleteIssueAsync(int id);
        Task<IssueStatistics> GetIssueStatisticsAsync();
    }
}
