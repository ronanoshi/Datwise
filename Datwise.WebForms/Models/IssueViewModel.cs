using System;

namespace Datwise.WebForms.Models
{
    public class IssueViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime ReportedDate { get; set; }
        public string ReportedBy { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
