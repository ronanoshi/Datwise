-- Datwise Security & Safety Control Panel - SQLite Database Schema
-- This script creates the database schema for SQLite

-- Create Examples table (for existing example functionality)
CREATE TABLE IF NOT EXISTS Examples (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT
);

-- Create Issues table for security and safety incident tracking
CREATE TABLE IF NOT EXISTS Issues (
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
CREATE INDEX IF NOT EXISTS idx_issues_status ON Issues(Status);
CREATE INDEX IF NOT EXISTS idx_issues_severity ON Issues(Severity);
CREATE INDEX IF NOT EXISTS idx_issues_reported_date ON Issues(ReportedDate);
CREATE INDEX IF NOT EXISTS idx_issues_status_severity ON Issues(Status, Severity);

-- Insert sample data for testing
INSERT INTO Issues (Title, Description, Severity, Status, ReportedBy, Department, Location)
VALUES 
    ('Fire Extinguisher Missing', 'Fire extinguisher removed from hallway near stairwell', 'High', 'Open', 'John Smith', 'Facilities', 'Building A - Floor 2'),
    ('Wet Floor Hazard', 'Water spill in cafeteria not marked', 'Medium', 'Open', 'Jane Doe', 'Operations', 'Building B - Cafeteria'),
    ('Equipment Malfunction', 'Safety alarm system not responding', 'Critical', 'In Progress', 'Mike Johnson', 'Maintenance', 'Building A - Floor 1'),
    ('First Aid Kit Empty', 'First aid kit on Floor 3 is depleted', 'Low', 'Open', 'Sarah Wilson', 'Facilities', 'Building C - Floor 3'),
    ('Broken Handrail', 'Stairwell handrail loose and needs replacement', 'High', 'Open', 'Tom Brown', 'Maintenance', 'Building A - Stairwell');
