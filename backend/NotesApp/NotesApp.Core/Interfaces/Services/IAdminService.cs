using NotesApp.Core.DTOs.Auth;

namespace NotesApp.Core.Interfaces.Services;

/// <summary>
/// Service interface for administrative operations.
/// </summary>
public interface IAdminService
{
    Task<IEnumerable<UserProfileResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default);
}
