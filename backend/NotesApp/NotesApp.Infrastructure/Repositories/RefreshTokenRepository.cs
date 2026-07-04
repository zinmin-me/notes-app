using Dapper;
using NotesApp.Core.Entities;
using NotesApp.Core.Interfaces.Repositories;
using NotesApp.Infrastructure.Data;

namespace NotesApp.Infrastructure.Repositories;

/// <summary>
/// Dapper implementation of the refresh token repository.
/// </summary>
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DapperContext _context;

    public RefreshTokenRepository(DapperContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT Id, UserId, Token, Expires, Revoked, CreatedAt
            FROM RefreshTokens
            WHERE Token = @Token";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<RefreshToken>(
            new CommandDefinition(sql, new { Token = token }, cancellationToken: cancellationToken));
    }

    /// <inheritdoc />
    public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO RefreshTokens (Id, UserId, Token, Expires, CreatedAt)
            OUTPUT INSERTED.Id, INSERTED.UserId, INSERTED.Token, INSERTED.Expires, INSERTED.Revoked, INSERTED.CreatedAt
            VALUES (@Id, @UserId, @Token, @Expires, SYSUTCDATETIME())";

        refreshToken.Id = Guid.NewGuid();

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<RefreshToken>(
            new CommandDefinition(sql, refreshToken, cancellationToken: cancellationToken));
    }

    /// <inheritdoc />
    public async Task RevokeAsync(string token, CancellationToken cancellationToken = default)
    {
        const string sql = "UPDATE RefreshTokens SET Revoked = SYSUTCDATETIME() WHERE Token = @Token AND Revoked IS NULL";

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(sql, new { Token = token }, cancellationToken: cancellationToken));
    }

    /// <inheritdoc />
    public async Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        const string sql = "UPDATE RefreshTokens SET Revoked = SYSUTCDATETIME() WHERE UserId = @UserId AND Revoked IS NULL";

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(sql, new { UserId = userId }, cancellationToken: cancellationToken));
    }
}
