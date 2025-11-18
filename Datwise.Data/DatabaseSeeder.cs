using System;
using System.Linq;
using Datwise.Models;
using Microsoft.EntityFrameworkCore;

namespace Datwise.Data
{
    public static class DatabaseSeeder
    {
        public static void SeedTestData(this DatwiseDbContext context)
        {
            // Only seed if the Issues table is empty
            if (context.Issues.Any())
            {
                return;
            }

            var sampleIssues = new[]
            {
                new Issue
                {
                    Title = "Fire Extinguisher Missing",
                    Description = "Fire extinguisher removed from hallway near stairwell",
                    Severity = "High",
                    Status = "Open",
                    ReportedBy = "John Smith",
                    Department = "Facilities",
                    Location = "Building A - Floor 2",
                    ReportedDate = DateTime.UtcNow.AddDays(-5)
                },
                new Issue
                {
                    Title = "Wet Floor Hazard",
                    Description = "Water spill in cafeteria not marked",
                    Severity = "Medium",
                    Status = "Open",
                    ReportedBy = "Jane Doe",
                    Department = "Operations",
                    Location = "Building B - Cafeteria",
                    ReportedDate = DateTime.UtcNow.AddDays(-3)
                },
                new Issue
                {
                    Title = "Equipment Malfunction",
                    Description = "Safety alarm system not responding",
                    Severity = "Critical",
                    Status = "In Progress",
                    ReportedBy = "Mike Johnson",
                    Department = "Maintenance",
                    Location = "Building A - Floor 1",
                    ReportedDate = DateTime.UtcNow.AddDays(-1)
                },
                new Issue
                {
                    Title = "First Aid Kit Empty",
                    Description = "First aid kit on Floor 3 is depleted",
                    Severity = "Low",
                    Status = "Open",
                    ReportedBy = "Sarah Wilson",
                    Department = "Facilities",
                    Location = "Building C - Floor 3",
                    ReportedDate = DateTime.UtcNow.AddHours(-12)
                },
                new Issue
                {
                    Title = "Broken Handrail",
                    Description = "Stairwell handrail loose and needs replacement",
                    Severity = "High",
                    Status = "Open",
                    ReportedBy = "Tom Brown",
                    Department = "Maintenance",
                    Location = "Building A - Stairwell",
                    ReportedDate = DateTime.UtcNow.AddHours(-6)
                }
            };

            context.Issues.AddRange(sampleIssues);
            context.SaveChanges();
        }
    }
}
