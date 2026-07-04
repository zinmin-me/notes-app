using Dapper;
using NotesApp.Core.Entities;
using NotesApp.Core.Interfaces.Repositories;
using NotesApp.Infrastructure.Data;

namespace NotesApp.Infrastructure.Repositories;

/// <summary>
/// Dapper implementation of the user repository.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT Id, Username, Email, PasswordHash, Role, CreatedAt FROM Users WHERE Id = @Id";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    /// <inheritdoc />
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT Id, Username, Email, PasswordHash, Role, CreatedAt FROM Users WHERE Email = @Email";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition(sql, new { Email = email }, cancellationToken: cancellationToken));
    }

    /// <inheritdoc />
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT Id, Username, Email, PasswordHash, Role, CreatedAt FROM Users WHERE Username = @Username";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition(sql, new { Username = username }, cancellationToken: cancellationToken));
    }

    /// <inheritdoc />
    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO Users (Id, Username, Email, PasswordHash, Role, CreatedAt)
            OUTPUT INSERTED.Id, INSERTED.Username, INSERTED.Email, INSERTED.PasswordHash, INSERTED.Role, INSERTED.CreatedAt
            VALUES (@Id, @Username, @Email, @PasswordHash, @Role, SYSUTCDATETIME())";

        user.Id = Guid.NewGuid();

        using var connection = _context.CreateConnection();
        var createdUser = await connection.QuerySingleAsync<User>(
            new CommandDefinition(sql, user, cancellationToken: cancellationToken));

        return createdUser;
    }

    /// <inheritdoc />
    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT COUNT(1) FROM Users WHERE Email = @Email";

        using var connection = _context.CreateConnection();
        var count = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(sql, new { Email = email }, cancellationToken: cancellationToken));

        return count > 0;
    }

    /// <inheritdoc />
    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT COUNT(1) FROM Users WHERE Username = @Username";

        using var connection = _context.CreateConnection();
        var count = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(sql, new { Username = username }, cancellationToken: cancellationToken));

        return count > 0;
    }
}
