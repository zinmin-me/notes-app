using NotesApp.Core.Entities;

namespace NotesApp.Core.Interfaces.Repositories;

/// <summary>
/// Repository interface for user data access operations.
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetCountJoinedTodayAsync(CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);
}
