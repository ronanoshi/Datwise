using Datwise.Models;

namespace Datwise.Contracts
{
    public interface IIssueRepository
    {
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Issue>> GetOpenIssuesAsync();
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Issue>> GetIssuesBySeverityAsync(string severity);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Issue>> GetIssuesByStatusAsync(string status);
        System.Threading.Tasks.Task<Issue?> GetIssueByIdAsync(int id);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Issue>> GetAllIssuesAsync();
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Issue>> GetIssuesAsync(string? status = null, string? severity = null, string? sortBy = null, bool sortDescending = false);
        System.Threading.Tasks.Task<int> CreateIssueAsync(Issue issue);
        System.Threading.Tasks.Task<bool> UpdateIssueAsync(Issue issue);
        System.Threading.Tasks.Task<bool> DeleteIssueAsync(int id);
        System.Threading.Tasks.Task<IssueStatistics> GetIssueStatisticsAsync();
    }
}
