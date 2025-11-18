using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Datwise.Models;
using Datwise.Contracts;

namespace Datwise.Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository;

        public IssueService(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public async Task<IEnumerable<Issue>> GetOpenIssuesAsync()
        {
            return await _issueRepository.GetOpenIssuesAsync();
        }

        public async Task<IEnumerable<Issue>> GetIssuesBySeverityAsync(string severity)
        {
            return await _issueRepository.GetIssuesBySeverityAsync(severity);
        }

        public async Task<Issue?> GetIssueByIdAsync(int id)
        {
            return await _issueRepository.GetIssueByIdAsync(id);
        }

        public async Task<IEnumerable<Issue>> GetIssuesAsync(string? status = null, string? severity = null, string? sortBy = null, bool sortDescending = false)
        {
            return await _issueRepository.GetIssuesAsync(status, severity, sortBy, sortDescending);
        }

        public async Task<int> CreateIssueAsync(Issue issue)
        {
            if (string.IsNullOrWhiteSpace(issue.Title))
                throw new ArgumentException("Title is required");

            if (string.IsNullOrWhiteSpace(issue.Description))
                throw new ArgumentException("Description is required");

            if (string.IsNullOrWhiteSpace(issue.ReportedBy))
                throw new ArgumentException("ReportedBy is required");

            return await _issueRepository.CreateIssueAsync(issue);
        }

        public async Task<bool> UpdateIssueAsync(Issue issue)
        {
            if (issue.Id <= 0)
                throw new ArgumentException("Invalid issue ID");

            return await _issueRepository.UpdateIssueAsync(issue);
        }

        public async Task<bool> DeleteIssueAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid issue ID");

            return await _issueRepository.DeleteIssueAsync(id);
        }

        public async Task<IssueStatistics> GetIssueStatisticsAsync()
        {
            return await _issueRepository.GetIssueStatisticsAsync();
        }
    }
}
