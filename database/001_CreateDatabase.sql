-- ============================================
-- Notes Management System
-- Database Creation Script
-- ============================================

USE [master];
GO

-- Drop database if it exists (development only)
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'NotesAppDb')
BEGIN
    ALTER DATABASE [NotesAppDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [NotesAppDb];
END
GO

-- Create database
CREATE DATABASE [NotesAppDb];
GO

USE [NotesAppDb];
GO

PRINT 'Database [NotesAppDb] created successfully.';
GO
