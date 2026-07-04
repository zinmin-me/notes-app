using System.Text;
using Dapper;
using NotesApp.Core.DTOs.Notes;
using NotesApp.Core.Entities;
using NotesApp.Core.Enums;
using NotesApp.Core.Interfaces.Repositories;
using NotesApp.Infrastructure.Data;

namespace NotesApp.Infrastructure.Repositories;

/// <summary>
/// Dapper implementation of the note repository with dynamic query building
/// for search, filter, sort, and pagination.
/// </summary>
public class NoteRepository : INoteRepository
{
    private readonly DapperContext _context;

    public NoteRepository(DapperContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt
            FROM Notes
            WHERE Id = @Id";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Note>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    /// <inheritdoc />
    public async Task<Note?> GetByIdAndUserIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt
            FROM Notes
            WHERE Id = @Id AND UserId = @UserId";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Note>(
            new CommandDefinition(sql, new { Id = id, UserId = userId }, cancellationToken: cancellationToken));
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Note> Notes, int TotalCount)> GetAllByUserIdAsync(
        Guid userId, NoteQueryParameters parameters, CancellationToken cancellationToken = default)
    {
        var dynamicParams = new DynamicParameters();
        dynamicParams.Add("UserId", userId);

        var whereClause = new StringBuilder("WHERE UserId = @UserId");

        // Search filter
        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            whereClause.Append(" AND (LOWER(Title) LIKE @Search OR LOWER(Content) LIKE @Search)");
            dynamicParams.Add("Search", $"%{parameters.Search.ToLower()}%");
        }

        // Date filter
        switch (parameters.DateFilter)
        {
            case DateFilter.Today:
                whereClause.Append(" AND CAST(CreatedAt AS DATE) = CAST(SYSUTCDATETIME() AS DATE)");
                break;
            case DateFilter.Last7Days:
                whereClause.Append(" AND CreatedAt >= DATEADD(DAY, -7, SYSUTCDATETIME())");
                break;
            case DateFilter.Last30Days:
                whereClause.Append(" AND CreatedAt >= DATEADD(DAY, -30, SYSUTCDATETIME())");
                break;
        }

        // Sort column (whitelist to prevent SQL injection)
        var sortColumn = parameters.SortBy switch
        {
            SortBy.Title => "Title",
            SortBy.UpdatedAt => "UpdatedAt",
            _ => "CreatedAt"
        };

        var sortDirection = parameters.SortOrder == SortOrder.Ascending ? "ASC" : "DESC";

        // Count query
        var countSql = $"SELECT COUNT(1) FROM Notes {whereClause}";

        // Data query with pagination
        var dataSql = $@"
            SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt
            FROM Notes
            {whereClause}
            ORDER BY {sortColumn} {sortDirection}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        dynamicParams.Add("Offset", (parameters.Page - 1) * parameters.PageSize);
        dynamicParams.Add("PageSize", parameters.PageSize);

        using var connection = _context.CreateConnection();

        var totalCount = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(countSql, dynamicParams, cancellationToken: cancellationToken));

        var notes = await connection.QueryAsync<Note>(
            new CommandDefinition(dataSql, dynamicParams, cancellationToken: cancellationToken));

        return (notes, totalCount);
    }

    /// <inheritdoc />
    public async Task<Note> CreateAsync(Note note, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO Notes (Id, UserId, Title, Content, CreatedAt, UpdatedAt)
            OUTPUT INSERTED.Id, INSERTED.UserId, INSERTED.Title, INSERTED.Content, INSERTED.CreatedAt, INSERTED.UpdatedAt
            VALUES (@Id, @UserId, @Title, @Content, SYSUTCDATETIME(), SYSUTCDATETIME())";

        note.Id = Guid.NewGuid();

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<Note>(
            new CommandDefinition(sql, note, cancellationToken: cancellationToken));
    }

    /// <inheritdoc />
    public async Task<Note> UpdateAsync(Note note, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE Notes
            SET Title = @Title, Content = @Content, UpdatedAt = SYSUTCDATETIME()
            OUTPUT INSERTED.Id, INSERTED.UserId, INSERTED.Title, INSERTED.Content, INSERTED.CreatedAt, INSERTED.UpdatedAt
            WHERE Id = @Id AND UserId = @UserId";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<Note>(
            new CommandDefinition(sql, note, cancellationToken: cancellationToken));
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId";

        using var connection = _context.CreateConnection();
        var affected = await connection.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id, UserId = userId }, cancellationToken: cancellationToken));

        return affected > 0;
    }
}
