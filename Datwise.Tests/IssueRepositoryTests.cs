using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Datwise.Models;
using Datwise.Data;
using Microsoft.EntityFrameworkCore;

namespace Datwise.Tests
{
    public class IssueRepositoryTests
    {
        private DatwiseDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DatwiseDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new DatwiseDbContext(options);
        }

        private List<Issue> GetSampleIssues()
        {
            return new List<Issue>
            {
                new Issue
                {
                    Id = 1,
                    Title = "Fire Extinguisher Missing",
                    Description = "Fire extinguisher removed from hallway",
                    Severity = "High",
                    Status = "Open",
                    ReportedBy = "John Smith",
                    Department = "Facilities",
                    Location = "Building A",
                    ReportedDate = DateTime.UtcNow.AddDays(-5)
                },
                new Issue
                {
                    Id = 2,
                    Title = "Wet Floor Hazard",
                    Description = "Water spill in cafeteria",
                    Severity = "Medium",
                    Status = "Open",
                    ReportedBy = "Jane Doe",
                    Department = "Operations",
                    Location = "Building B",
                    ReportedDate = DateTime.UtcNow.AddDays(-3)
                },
                new Issue
                {
                    Id = 3,
                    Title = "Equipment Malfunction",
                    Description = "Safety alarm system not responding",
                    Severity = "Critical",
                    Status = "In Progress",
                    ReportedBy = "Mike Johnson",
                    Department = "Maintenance",
                    Location = "Building A",
                    ReportedDate = DateTime.UtcNow.AddDays(-1)
                }
            };
        }

        [Fact]
        public async Task GetOpenIssuesAsync_ReturnsOnlyOpenAndInProgressIssues()
        {
            // Arrange
            var context = GetDbContext();
            var issues = GetSampleIssues();
            issues.Add(new Issue { Id = 4, Title = "Resolved Issue", Status = "Resolved", Severity = "Low", ReportedBy = "Test" });
            context.Issues.AddRange(issues);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act
            var result = await repository.GetOpenIssuesAsync();

            // Assert
            Assert.Equal(3, result.Count());
            Assert.All(result, issue => Assert.True(issue.Status == "Open" || issue.Status == "In Progress"));
        }

        [Fact]
        public async Task GetIssuesBySeverityAsync_ReturnIssuesWithMatchingSeverity()
        {
            // Arrange
            var context = GetDbContext();
            var issues = GetSampleIssues();
            context.Issues.AddRange(issues);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act
            var result = await repository.GetIssuesBySeverityAsync("High");

            // Assert
            Assert.Single(result);
            Assert.All(result, issue => Assert.Equal("High", issue.Severity));
        }

        [Fact]
        public async Task GetIssuesAsync_WithStatusFilter_ReturnsFilteredIssues()
        {
            // Arrange
            var context = GetDbContext();
            var issues = GetSampleIssues();
            context.Issues.AddRange(issues);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act
            var result = await repository.GetIssuesAsync(status: "In Progress");

            // Assert
            Assert.Single(result);
            Assert.Equal("In Progress", result.First().Status);
        }

        [Fact]
        public async Task GetIssuesAsync_WithSorting_ReturnsSortedIssues()
        {
            // Arrange
            var context = GetDbContext();
            var issues = GetSampleIssues();
            context.Issues.AddRange(issues);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act
            var resultAsc = await repository.GetIssuesAsync(sortBy: "title", sortDescending: false);
            var resultDesc = await repository.GetIssuesAsync(sortBy: "title", sortDescending: true);

            // Assert
            var ascList = resultAsc.ToList();
            var descList = resultDesc.ToList();
            
            Assert.True(string.Compare(ascList[0].Title, ascList[1].Title) <= 0);
            Assert.True(string.Compare(descList[0].Title, descList[1].Title) >= 0);
        }

        [Fact]
        public async Task GetIssuesAsync_WithPrefixedSort_ParsesSortCorrectly()
        {
            // Arrange
            var context = GetDbContext();
            var issues = GetSampleIssues();
            context.Issues.AddRange(issues);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act
            var result = await repository.GetIssuesAsync(sortBy: "-title");

            // Assert
            var list = result.ToList();
            Assert.True(string.Compare(list[0].Title, list[1].Title) >= 0);
        }

        [Fact]
        public async Task CreateIssueAsync_AddsIssueToDatabase()
        {
            // Arrange
            var context = GetDbContext();
            var repository = new IssueRepository(context);
            var newIssue = new Issue
            {
                Title = "New Issue",
                Description = "Test",
                Severity = "High",
                Status = "Open",
                ReportedBy = "Test User",
                Department = "Test",
                Location = "Test"
            };

            // Act
            var id = await repository.CreateIssueAsync(newIssue);

            // Assert
            Assert.True(id > 0);
            var savedIssue = await repository.GetIssueByIdAsync(id);
            Assert.NotNull(savedIssue);
            Assert.Equal("New Issue", savedIssue.Title);
        }

        [Fact]
        public async Task UpdateIssueAsync_UpdatesExistingIssue()
        {
            // Arrange
            var context = GetDbContext();
            var issue = new Issue { Title = "Original", Severity = "Low", Status = "Open", ReportedBy = "Test" };
            context.Issues.Add(issue);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act
            issue.Title = "Updated";
            var success = await repository.UpdateIssueAsync(issue);

            // Assert
            Assert.True(success);
            var updated = await repository.GetIssueByIdAsync(issue.Id);
            Assert.Equal("Updated", updated.Title);
        }

        [Fact]
        public async Task DeleteIssueAsync_RemovesIssue()
        {
            // Arrange
            var context = GetDbContext();
            var issue = new Issue { Title = "To Delete", Severity = "Low", Status = "Open", ReportedBy = "Test" };
            context.Issues.Add(issue);
            await context.SaveChangesAsync();
            var issueId = issue.Id;
            var repository = new IssueRepository(context);

            // Act
            var success = await repository.DeleteIssueAsync(issueId);

            // Assert
            Assert.True(success);
            var deleted = await repository.GetIssueByIdAsync(issueId);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task GetIssueStatisticsAsync_ReturnsCorrectStatistics()
        {
            // Arrange
            var context = GetDbContext();
            var issues = GetSampleIssues();
            context.Issues.AddRange(issues);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act
            var stats = await repository.GetIssueStatisticsAsync();

            // Assert
            Assert.True(stats.TotalOpenIssues > 0);
            Assert.Equal(1, stats.CriticalIssuesCount);
            Assert.Equal(1, stats.HighSeverityCount);
        }

        [Fact]
        public async Task GetIssuesAsync_WithMultipleStatuses_ReturnsFilteredIssues()
        {
            // Arrange
            var context = GetDbContext();
            var issues = GetSampleIssues();
            issues.Add(new Issue { Id = 4, Title = "Resolved Issue", Status = "Resolved", Severity = "Low", ReportedBy = "Test" });
            context.Issues.AddRange(issues);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act
            var result = await repository.GetIssuesAsync(status: "Open,In Progress");

            // Assert
            Assert.Equal(3, result.Count());
            Assert.All(result, issue => Assert.True(issue.Status == "Open" || issue.Status == "In Progress"));
        }

        [Fact]
        public async Task GetIssuesAsync_WithMultipleSeverities_ReturnsFilteredIssues()
        {
            // Arrange
            var context = GetDbContext();
            var issues = GetSampleIssues();
            context.Issues.AddRange(issues);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act
            var result = await repository.GetIssuesAsync(severity: "High,Critical");

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, issue => Assert.True(issue.Severity == "High" || issue.Severity == "Critical"));
        }

        [Fact]
        public async Task GetIssuesAsync_WithMultipleStatusesAndSeverities_ReturnsFilteredIssues()
        {
            // Arrange
            var context = GetDbContext();
            var issues = GetSampleIssues();
            issues.Add(new Issue { Id = 4, Title = "Low Resolved", Status = "Resolved", Severity = "Low", ReportedBy = "Test" });
            context.Issues.AddRange(issues);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act
            var result = await repository.GetIssuesAsync(status: "Open,In Progress", severity: "High,Critical");

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, issue => Assert.True((issue.Status == "Open" || issue.Status == "In Progress") && 
                                                     (issue.Severity == "High" || issue.Severity == "Critical")));
        }

        [Fact]
        public async Task GetIssuesAsync_SortByAllFields_WorksCorrectly()
        {
            // Arrange
            var context = GetDbContext();
            var issues = GetSampleIssues();
            context.Issues.AddRange(issues);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act & Assert - Test each sortable field
            var sortFields = new[] { "id", "title", "severity", "status", "department", "location", "reportedby", "date", "reporteddate" };
            foreach (var field in sortFields)
            {
                var result = await repository.GetIssuesAsync(sortBy: field);
                Assert.NotEmpty(result);
                
                var resultDesc = await repository.GetIssuesAsync(sortBy: $"-{field}");
                Assert.NotEmpty(resultDesc);
            }
        }

        [Fact]
        public async Task GetIssueByIdAsync_WithValidId_ReturnsIssue()
        {
            // Arrange
            var context = GetDbContext();
            var issue = new Issue
            {
                Id = 1,
                Title = "Test Issue",
                Description = "Test",
                Severity = "High",
                Status = "Open",
                ReportedBy = "Test",
                Department = "Test",
                Location = "Test"
            };
            context.Issues.Add(issue);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act
            var result = await repository.GetIssueByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Issue", result.Title);
        }

        [Fact]
        public async Task GetIssueByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var context = GetDbContext();
            var repository = new IssueRepository(context);

            // Act
            var result = await repository.GetIssueByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateIssueAsync_SetsReportedDateToUtcNow()
        {
            // Arrange
            var context = GetDbContext();
            var repository = new IssueRepository(context);
            var beforeCreation = DateTime.UtcNow;
            var newIssue = new Issue
            {
                Title = "New Issue",
                Description = "Test",
                Severity = "High",
                Status = "Open",
                ReportedBy = "Test",
                Department = "Test",
                Location = "Test"
            };

            // Act
            var id = await repository.CreateIssueAsync(newIssue);
            var savedIssue = await repository.GetIssueByIdAsync(id);

            // Assert
            Assert.NotNull(savedIssue);
            Assert.True(savedIssue.ReportedDate >= beforeCreation);
            Assert.True(savedIssue.ReportedDate <= DateTime.UtcNow);
        }

        [Fact]
        public async Task GetAllIssuesAsync_ReturnsAllIssuesOrderedByDate()
        {
            // Arrange
            var context = GetDbContext();
            var issues = GetSampleIssues();
            context.Issues.AddRange(issues);
            await context.SaveChangesAsync();
            var repository = new IssueRepository(context);

            // Act
            var result = await repository.GetAllIssuesAsync();

            // Assert
            Assert.Equal(3, result.Count());
            var list = result.ToList();
            Assert.True(list[0].ReportedDate >= list[1].ReportedDate);
        }
    }
}
