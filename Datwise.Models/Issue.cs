namespace Datwise.Models
{
    /// <summary>
    /// Represents a security or safety issue/incident report
    /// </summary>
    public class Issue
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Severity levels: Low, Medium, High, Critical
        /// </summary>
        public string Severity { get; set; } = "Medium";
        
        /// <summary>
        /// Status: Open, In Progress, Resolved, Closed
        /// </summary>
        public string Status { get; set; } = "Open";
        
        public System.DateTime ReportedDate { get; set; } = System.DateTime.UtcNow;
        
        public string ReportedBy { get; set; } = string.Empty;
        
        public System.DateTime? ResolvedDate { get; set; }
        
        public string? ResolvedBy { get; set; }
        
        public string? ResolutionNotes { get; set; }
        
        public string Department { get; set; } = string.Empty;
        
        public string Location { get; set; } = string.Empty;
    }
}
