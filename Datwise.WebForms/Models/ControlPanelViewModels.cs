using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// This file is kept for backwards compatibility
// All view models have been split into their own files
#pragma warning disable CS0618

namespace Datwise.WebForms.Models
{
    // Obsolete: Use IssueViewModel instead
    [Obsolete("Use IssueViewModel instead", false)]
    public class ErrorViewModel : IssueViewModel
    {
    }

    // Obsolete: Use IssueStatisticsViewModel instead
    [Obsolete("Use IssueStatisticsViewModel instead", false)]
    public class ErrorStatisticsViewModel : IssueStatisticsViewModel
    {
    }
}
