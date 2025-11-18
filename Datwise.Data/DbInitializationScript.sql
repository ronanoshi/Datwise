-- Datwise Database Initialization Script for SQLite
-- This script creates all necessary tables for the Datwise application

-- Create Errors table
CREATE TABLE IF NOT EXISTS Errors (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT NOT NULL,
    Severity TEXT NOT NULL DEFAULT 'Medium',
    Status TEXT NOT NULL DEFAULT 'Open',
    ReportedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ReportedBy TEXT NOT NULL,
    ResolvedDate DATETIME,
    ResolvedBy TEXT,
    ResolutionNotes TEXT,
    Department TEXT,
    Location TEXT
);

-- Create indexes for better query performance
CREATE INDEX IF NOT EXISTS IX_Errors_Status ON Errors(Status);
CREATE INDEX IF NOT EXISTS IX_Errors_Severity ON Errors(Severity);
CREATE INDEX IF NOT EXISTS IX_Errors_ReportedDate ON Errors(ReportedDate);

-- Insert sample data for testing (optional)
-- INSERT INTO Errors (Title, Description, Severity, Status, ReportedBy, Department, Location)
-- VALUES ('Sample Critical Issue', 'This is a test critical issue', 'Critical', 'Open', 'Admin', 'Operations', 'Building A');

-- INSERT INTO Errors (Title, Description, Severity, Status, ReportedBy, Department, Location)
-- VALUES ('Sample High Priority Issue', 'This is a test high priority issue', 'High', 'Open', 'Admin', 'Facilities', 'Building B');

-- INSERT INTO Errors (Title, Description, Severity, Status, ReportedBy, Department, Location)
-- VALUES ('Sample Medium Priority Issue', 'This is a test medium priority issue', 'Medium', 'In Progress', 'Admin', 'Security', 'Entrance');

-- INSERT INTO Errors (Title, Description, Severity, Status, ReportedBy, Department, Location)
-- VALUES ('Sample Low Priority Issue', 'This is a test low priority issue', 'Low', 'Open', 'Admin', 'Administration', 'Office');
