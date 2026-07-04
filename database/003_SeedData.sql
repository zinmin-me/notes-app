-- ============================================
-- Notes Management System
-- Seed Data Script
-- ============================================
-- Passwords are BCrypt hashed. Both users use: Password@123
-- ============================================

USE [NotesAppDb];
GO

-- ============================================
-- Seed Users
-- ============================================
DECLARE @AdminId UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @UserId  UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222222';

-- BCrypt hash for 'Password@123'
DECLARE @PasswordHash NVARCHAR(500) = '$2a$11$NPZNiPCM9FRhTAbXsTnLPOqwMpmjbRjZva9SY5I.mhi4n0fbujx/G';

IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Email] = 'admin@notesapp.com')
BEGIN


    INSERT INTO [dbo].[Users] ([Id], [Username], [Email], [PasswordHash], [Role], [CreatedAt])
    VALUES
        (@AdminId, 'admin', 'admin@notesapp.com', @PasswordHash, 'Admin', SYSUTCDATETIME()),
        (@UserId, 'johndoe', 'john@notesapp.com', @PasswordHash, 'User', SYSUTCDATETIME());

    PRINT 'Seed users created successfully.';
END
GO

-- ============================================
-- Seed Notes
-- ============================================
DECLARE @AdminUserId UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @RegularUserId UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222222';

IF NOT EXISTS (SELECT 1 FROM [dbo].[Notes] WHERE [UserId] = @RegularUserId)
BEGIN
    INSERT INTO [dbo].[Notes] ([Id], [UserId], [Title], [Content], [CreatedAt], [UpdatedAt])
    VALUES
        (NEWID(), @RegularUserId, 'Getting Started with Vue 3',
         'Vue 3 introduces the Composition API which provides a more flexible way to organize component logic. Key features include ref(), reactive(), computed(), and watch(). The setup() function or <script setup> syntax allows for cleaner component composition.',
         DATEADD(DAY, -15, SYSUTCDATETIME()), DATEADD(DAY, -15, SYSUTCDATETIME())),

        (NEWID(), @RegularUserId, 'TypeScript Best Practices',
         'Always use strict mode. Prefer interfaces over types for object shapes. Use enums for fixed sets of values. Leverage generics for reusable code. Use utility types like Partial<T>, Pick<T>, and Omit<T>. Avoid using any - prefer unknown when the type is truly unknown.',
         DATEADD(DAY, -10, SYSUTCDATETIME()), DATEADD(DAY, -8, SYSUTCDATETIME())),

        (NEWID(), @RegularUserId, 'Clean Architecture Notes',
         'Clean Architecture separates concerns into layers: Domain (entities, value objects), Application (use cases, interfaces), Infrastructure (data access, external services), and Presentation (controllers, views). Dependencies always point inward toward the domain.',
         DATEADD(DAY, -7, SYSUTCDATETIME()), DATEADD(DAY, -5, SYSUTCDATETIME())),

        (NEWID(), @RegularUserId, 'Daily Standup Agenda',
         '1. What did I accomplish yesterday? 2. What am I working on today? 3. Are there any blockers? Remember to keep updates brief and focused. Time-box to 15 minutes.',
         DATEADD(DAY, -3, SYSUTCDATETIME()), DATEADD(DAY, -2, SYSUTCDATETIME())),

        (NEWID(), @RegularUserId, 'API Design Guidelines',
         'Use RESTful conventions. Use proper HTTP methods (GET, POST, PUT, DELETE). Return appropriate status codes. Use consistent response envelopes. Implement pagination for list endpoints. Version your APIs. Document with OpenAPI/Swagger.',
         DATEADD(DAY, -2, SYSUTCDATETIME()), DATEADD(DAY, -1, SYSUTCDATETIME())),

        (NEWID(), @RegularUserId, 'Docker Deployment Checklist',
         'Multi-stage builds for smaller images. Use .dockerignore. Set non-root user. Use health checks. Configure environment variables. Use docker-compose for multi-service apps. Tag images properly. Scan for vulnerabilities.',
         DATEADD(DAY, -1, SYSUTCDATETIME()), DATEADD(DAY, -1, SYSUTCDATETIME())),

        (NEWID(), @RegularUserId, 'Meeting Notes - Sprint Planning',
         'Sprint Goal: Complete user authentication module and notes CRUD. Story points committed: 34. Key deliverables: JWT auth, refresh tokens, note creation with rich text, search functionality. Sprint duration: 2 weeks.',
         SYSUTCDATETIME(), SYSUTCDATETIME()),

        (NEWID(), @RegularUserId, 'Learning Roadmap 2024',
         'Q1: Master Vue 3 Composition API and Pinia. Q2: Deep dive into ASP.NET Core and Clean Architecture. Q3: Cloud services (Azure/AWS). Q4: System design and microservices. Side goals: contribute to open source, write technical blog posts.',
         DATEADD(DAY, -20, SYSUTCDATETIME()), DATEADD(DAY, -12, SYSUTCDATETIME()));

    PRINT 'Seed notes for user [johndoe] created successfully.';
END


IF NOT EXISTS (SELECT 1 FROM [dbo].[Notes] WHERE [UserId] = @AdminUserId)
BEGIN
    INSERT INTO [dbo].[Notes] ([Id], [UserId], [Title], [Content], [CreatedAt], [UpdatedAt])
    VALUES
        (NEWID(), @AdminUserId, 'System Administration Guide',
         'Server maintenance schedule: Weekly backups every Sunday at 2 AM UTC. Database optimization runs monthly. Monitor disk space, CPU, and memory usage. Review security logs daily. Update SSL certificates before expiry.',
         DATEADD(DAY, -5, SYSUTCDATETIME()), DATEADD(DAY, -1, SYSUTCDATETIME())),

        (NEWID(), @AdminUserId, 'Security Audit Findings',
         'Completed security audit on 2024-01-15. Findings: 1) Implement rate limiting on auth endpoints. 2) Add CORS restrictions for production. 3) Enable HTTPS redirect. 4) Review JWT token expiry times. 5) Add input sanitization middleware.',
         DATEADD(DAY, -3, SYSUTCDATETIME()), DATEADD(DAY, -2, SYSUTCDATETIME()));

    PRINT 'Seed notes for user [admin] created successfully.';
END
GO

PRINT 'Seed data insertion completed.';
GO
