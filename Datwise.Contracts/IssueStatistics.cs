namespace Datwise.Contracts
{
    public class IssueStatistics
    {
        public int TotalOpenIssues { get; set; }
        public int CriticalIssuesCount { get; set; }
        public int HighSeverityCount { get; set; }
        public int MediumSeverityCount { get; set; }
        public int LowSeverityCount { get; set; }
        public System.DateTime LastIssueDate { get; set; }
        public int ResolvedThisMonth { get; set; }
    }
}
