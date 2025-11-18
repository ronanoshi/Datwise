using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Datwise.Models;
using Datwise.Services;
using Datwise.Contracts;

namespace Datwise.Tests
{
    public class IssueServiceTests
    {
        private Mock<IIssueRepository> GetMockRepository()
        {
            return new Mock<IIssueRepository>();
        }

        private List<Issue> GetSampleIssues()
        {
            return new List<Issue>
            {
                new Issue
                {
                    Id = 1,
                    Title = "Issue 1",
                    Description = "Description 1",
                    Severity = "High",
                    Status = "Open",
                    ReportedBy = "User 1",
                    Department = "Dept 1",
                    Location = "Location 1",
                    ReportedDate = DateTime.UtcNow
                },
                new Issue
                {
                    Id = 2,
                    Title = "Issue 2",
                    Description = "Description 2",
                    Severity = "Low",
                    Status = "Open",
                    ReportedBy = "User 2",
                    Department = "Dept 2",
                    Location = "Location 2",
                    ReportedDate = DateTime.UtcNow
                }
            };
        }

        [Fact]
        public async Task GetOpenIssuesAsync_CallsRepository()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var issues = GetSampleIssues();
            mockRepo.Setup(r => r.GetOpenIssuesAsync()).ReturnsAsync(issues);
            var service = new IssueService(mockRepo.Object);

            // Act
            var result = await service.GetOpenIssuesAsync();

            // Assert
            Assert.Equal(2, result.Count());
            mockRepo.Verify(r => r.GetOpenIssuesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetIssuesBySeverityAsync_CallsRepository()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var issues = GetSampleIssues().Where(i => i.Severity == "High").ToList();
            mockRepo.Setup(r => r.GetIssuesBySeverityAsync("High")).ReturnsAsync(issues);
            var service = new IssueService(mockRepo.Object);

            // Act
            var result = await service.GetIssuesBySeverityAsync("High");

            // Assert
            Assert.Single(result);
            mockRepo.Verify(r => r.GetIssuesBySeverityAsync("High"), Times.Once);
        }

        [Fact]
        public async Task GetIssuesAsync_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var issues = GetSampleIssues();
            mockRepo.Setup(r => r.GetIssuesAsync("Open", "High", "date", true)).ReturnsAsync(issues);
            var service = new IssueService(mockRepo.Object);

            // Act
            var result = await service.GetIssuesAsync("Open", "High", "date", true);

            // Assert
            Assert.Equal(2, result.Count());
            mockRepo.Verify(r => r.GetIssuesAsync("Open", "High", "date", true), Times.Once);
        }

        [Fact]
        public async Task CreateIssueAsync_WithValidData_CreatesIssue()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            mockRepo.Setup(r => r.CreateIssueAsync(It.IsAny<Issue>())).ReturnsAsync(1);
            var service = new IssueService(mockRepo.Object);
            var newIssue = new Issue
            {
                Title = "New Issue",
                Description = "Description",
                Severity = "High",
                Status = "Open",
                ReportedBy = "User",
                Department = "Dept",
                Location = "Location"
            };

            // Act
            var result = await service.CreateIssueAsync(newIssue);

            // Assert
            Assert.Equal(1, result);
            mockRepo.Verify(r => r.CreateIssueAsync(newIssue), Times.Once);
        }

        [Fact]
        public async Task CreateIssueAsync_WithMissingTitle_ThrowsException()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var service = new IssueService(mockRepo.Object);
            var invalidIssue = new Issue
            {
                Title = "",
                Description = "Description",
                Severity = "High",
                Status = "Open",
                ReportedBy = "User",
                Department = "Dept",
                Location = "Location"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateIssueAsync(invalidIssue));
        }

        [Fact]
        public async Task CreateIssueAsync_WithMissingDescription_ThrowsException()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var service = new IssueService(mockRepo.Object);
            var invalidIssue = new Issue
            {
                Title = "Title",
                Description = "",
                Severity = "High",
                Status = "Open",
                ReportedBy = "User",
                Department = "Dept",
                Location = "Location"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateIssueAsync(invalidIssue));
        }

        [Fact]
        public async Task CreateIssueAsync_WithMissingReportedBy_ThrowsException()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var service = new IssueService(mockRepo.Object);
            var invalidIssue = new Issue
            {
                Title = "Title",
                Description = "Description",
                Severity = "High",
                Status = "Open",
                ReportedBy = "",
                Department = "Dept",
                Location = "Location"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateIssueAsync(invalidIssue));
        }

        [Fact]
        public async Task UpdateIssueAsync_WithValidId_UpdatesIssue()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            mockRepo.Setup(r => r.UpdateIssueAsync(It.IsAny<Issue>())).ReturnsAsync(true);
            var service = new IssueService(mockRepo.Object);
            var issue = new Issue { Id = 1, Title = "Updated", Severity = "High", ReportedBy = "User" };

            // Act
            var result = await service.UpdateIssueAsync(issue);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateIssueAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var service = new IssueService(mockRepo.Object);
            var issue = new Issue { Id = 0, Title = "Title", Severity = "High", ReportedBy = "User" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateIssueAsync(issue));
        }

        [Fact]
        public async Task DeleteIssueAsync_WithValidId_DeletesIssue()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            mockRepo.Setup(r => r.DeleteIssueAsync(1)).ReturnsAsync(true);
            var service = new IssueService(mockRepo.Object);

            // Act
            var result = await service.DeleteIssueAsync(1);

            // Assert
            Assert.True(result);
            mockRepo.Verify(r => r.DeleteIssueAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteIssueAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var service = new IssueService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteIssueAsync(0));
        }

        [Fact]
        public async Task GetIssueStatisticsAsync_ReturnsStatistics()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var stats = new IssueStatistics
            {
                TotalOpenIssues = 5,
                CriticalIssuesCount = 1,
                HighSeverityCount = 2,
                ResolvedThisMonth = 3
            };
            mockRepo.Setup(r => r.GetIssueStatisticsAsync()).ReturnsAsync(stats);
            var service = new IssueService(mockRepo.Object);

            // Act
            var result = await service.GetIssueStatisticsAsync();

            // Assert
            Assert.Equal(5, result.TotalOpenIssues);
            Assert.Equal(1, result.CriticalIssuesCount);
        }

        [Fact]
        public async Task GetIssuesAsync_WithEmptyFilters_CallsRepositoryWithNullValues()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var issues = GetSampleIssues();
            mockRepo.Setup(r => r.GetIssuesAsync(null, null, null, false)).ReturnsAsync(issues);
            var service = new IssueService(mockRepo.Object);

            // Act
            var result = await service.GetIssuesAsync(null, null, null, false);

            // Assert
            Assert.Equal(2, result.Count());
            mockRepo.Verify(r => r.GetIssuesAsync(null, null, null, false), Times.Once);
        }

        [Fact]
        public async Task GetIssueByIdAsync_CallsRepository()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var issue = GetSampleIssues().First();
            mockRepo.Setup(r => r.GetIssueByIdAsync(1)).ReturnsAsync(issue);
            var service = new IssueService(mockRepo.Object);

            // Act
            var result = await service.GetIssueByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(issue.Id, result.Id);
            mockRepo.Verify(r => r.GetIssueByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task UpdateIssueAsync_CallsRepository()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            mockRepo.Setup(r => r.UpdateIssueAsync(It.IsAny<Issue>())).ReturnsAsync(true);
            var service = new IssueService(mockRepo.Object);
            var issue = new Issue { Id = 1, Title = "Updated", Severity = "High", Status = "Open", ReportedBy = "User", Description = "Desc" };

            // Act
            var result = await service.UpdateIssueAsync(issue);

            // Assert
            Assert.True(result);
            mockRepo.Verify(r => r.UpdateIssueAsync(issue), Times.Once);
        }

        [Fact]
        public async Task DeleteIssueAsync_WithValidId_CallsRepository()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            mockRepo.Setup(r => r.DeleteIssueAsync(5)).ReturnsAsync(true);
            var service = new IssueService(mockRepo.Object);

            // Act
            var result = await service.DeleteIssueAsync(5);

            // Assert
            Assert.True(result);
            mockRepo.Verify(r => r.DeleteIssueAsync(5), Times.Once);
        }

        [Fact]
        public async Task CreateIssueAsync_ValidatesAllRequiredFields()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var service = new IssueService(mockRepo.Object);

            // Act & Assert - Title validation
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateIssueAsync(
                new Issue { Title = "  ", Description = "Desc", ReportedBy = "User", Severity = "High", Status = "Open", Department = "Dept", Location = "Loc" }
            ));

            // Act & Assert - Description validation
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateIssueAsync(
                new Issue { Title = "Title", Description = "  ", ReportedBy = "User", Severity = "High", Status = "Open", Department = "Dept", Location = "Loc" }
            ));

            // Act & Assert - ReportedBy validation
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateIssueAsync(
                new Issue { Title = "Title", Description = "Desc", ReportedBy = "  ", Severity = "High", Status = "Open", Department = "Dept", Location = "Loc" }
            ));
        }

        [Fact]
        public async Task GetIssuesAsync_WithMultipleStatusesAndSeverities_CallsRepositoryCorrectly()
        {
            // Arrange
            var mockRepo = GetMockRepository();
            var issues = GetSampleIssues();
            mockRepo.Setup(r => r.GetIssuesAsync("Open,In Progress", "High,Critical", "date", true)).ReturnsAsync(issues);
            var service = new IssueService(mockRepo.Object);

            // Act
            var result = await service.GetIssuesAsync("Open,In Progress", "High,Critical", "date", true);

            // Assert
            Assert.Equal(2, result.Count());
            mockRepo.Verify(r => r.GetIssuesAsync("Open,In Progress", "High,Critical", "date", true), Times.Once);
        }
    }
}
