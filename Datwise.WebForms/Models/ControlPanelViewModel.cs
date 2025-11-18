using System;
using System.Collections.Generic;

namespace Datwise.WebForms.Models
{
    public class ControlPanelViewModel
    {
        public IssueStatisticsViewModel Statistics { get; set; } = new();
        public List<IssueViewModel> OpenIssues { get; set; } = new();
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
    }
}
