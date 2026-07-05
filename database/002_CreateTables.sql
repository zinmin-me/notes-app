-- ============================================
-- Notes Management System
-- Table Creation Script
-- ============================================

-- USE [NotesAppDb];
-- GO

-- ============================================
-- Users Table
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id]           UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
        [Username]     NVARCHAR(50)     NOT NULL,
        [Email]        NVARCHAR(256)    NOT NULL,
        [PasswordHash] NVARCHAR(500)    NOT NULL,
        [Role]         NVARCHAR(20)     NOT NULL DEFAULT 'User',
        [CreatedAt]    DATETIME2(7)     NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [UQ_Users_Email] UNIQUE ([Email]),
        CONSTRAINT [UQ_Users_Username] UNIQUE ([Username]),
        CONSTRAINT [CK_Users_Role] CHECK ([Role] IN ('User', 'Admin'))
    );

    PRINT 'Table [Users] created successfully.';
END
GO

-- ============================================
-- RefreshTokens Table
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RefreshTokens')
BEGIN
    CREATE TABLE [dbo].[RefreshTokens] (
        [Id]        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
        [UserId]    UNIQUEIDENTIFIER NOT NULL,
        [Token]     NVARCHAR(500)    NOT NULL,
        [Expires]   DATETIME2(7)     NOT NULL,
        [Revoked]   DATETIME2(7)     NULL,
        [CreatedAt] DATETIME2(7)     NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY ([UserId])
            REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE,
        CONSTRAINT [UQ_RefreshTokens_Token] UNIQUE ([Token])
    );

    PRINT 'Table [RefreshTokens] created successfully.';
END
GO

-- ============================================
-- Notes Table
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notes')
BEGIN
    CREATE TABLE [dbo].[Notes] (
        [Id]        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
        [UserId]    UNIQUEIDENTIFIER NOT NULL,
        [Title]     NVARCHAR(200)    NOT NULL,
        [Content]   NVARCHAR(MAX)    NULL,
        [CreatedAt] DATETIME2(7)     NOT NULL DEFAULT SYSUTCDATETIME(),
        [UpdatedAt] DATETIME2(7)     NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT [PK_Notes] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_Notes_Users] FOREIGN KEY ([UserId])
            REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE
    );

    PRINT 'Table [Notes] created successfully.';
END
GO

-- ============================================
-- Indexes
-- ============================================

-- Users indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Email')
    CREATE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users]([Email]);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Username')
    CREATE NONCLUSTERED INDEX [IX_Users_Username] ON [dbo].[Users]([Username]);
GO

-- RefreshTokens indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_RefreshTokens_Token')
    CREATE NONCLUSTERED INDEX [IX_RefreshTokens_Token] ON [dbo].[RefreshTokens]([Token]);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_RefreshTokens_UserId')
    CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserId] ON [dbo].[RefreshTokens]([UserId]);
GO

-- Notes indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Notes_UserId')
    CREATE NONCLUSTERED INDEX [IX_Notes_UserId] ON [dbo].[Notes]([UserId]);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Notes_CreatedAt')
    CREATE NONCLUSTERED INDEX [IX_Notes_CreatedAt] ON [dbo].[Notes]([CreatedAt] DESC);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Notes_UpdatedAt')
    CREATE NONCLUSTERED INDEX [IX_Notes_UpdatedAt] ON [dbo].[Notes]([UpdatedAt] DESC);
GO

-- Full-text search support (if full-text is installed)
IF EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = 'NotesCatalog')
    DROP FULLTEXT CATALOG [NotesCatalog];
GO

-- Note: Full-text search requires Full-Text Search feature to be installed.
-- Uncomment below if available:
-- CREATE FULLTEXT CATALOG [NotesCatalog] AS DEFAULT;
-- CREATE FULLTEXT INDEX ON [dbo].[Notes]([Title], [Content]) KEY INDEX [PK_Notes] ON [NotesCatalog];

PRINT 'All indexes created successfully.';
GO
