using NotesApp.Core.DTOs.Auth;
using NotesApp.Core.DTOs.Admin;

namespace NotesApp.Core.Interfaces.Services;

/// <summary>
/// Service interface for administrative operations.
/// </summary>
public interface IAdminService
{
    Task<IEnumerable<UserProfileResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<AdminDashboardStatsDto> GetDashboardStatsAsync(CancellationToken cancellationToken = default);
}
