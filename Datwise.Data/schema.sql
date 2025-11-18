-- Datwise SQLite schema
CREATE TABLE IF NOT EXISTS Examples (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL
);

-- Example of a simple stored procedure equivalent in SQLite (as a trigger or view)
-- SQLite does not support stored procedures, but you can use triggers or user-defined functions.
-- Example: Insert trigger (for demonstration)
CREATE TRIGGER IF NOT EXISTS trg_Examples_Insert
AFTER INSERT ON Examples
BEGIN
    -- Custom logic here
END;
