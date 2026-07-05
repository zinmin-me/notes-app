using NotesApp.Core.DTOs.Auth;
using NotesApp.Core.DTOs.Admin;
using NotesApp.Core.Interfaces.Repositories;
using NotesApp.Core.Interfaces.Services;

namespace NotesApp.Application.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly INoteRepository _noteRepository;

    public AdminService(IUserRepository userRepository, INoteRepository noteRepository)
    {
        _userRepository = userRepository;
        _noteRepository = noteRepository;
    }

    public async Task<IEnumerable<UserProfileResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        
        return users.Select(u => new UserProfileResponse
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            Role = u.Role,
            CreatedAt = u.CreatedAt
        });
    }
    public async Task<AdminDashboardStatsDto> GetDashboardStatsAsync(CancellationToken cancellationToken = default)
    {
        var totalUsers = await _userRepository.GetTotalCountAsync(cancellationToken);
        var totalNotes = await _noteRepository.GetTotalCountAsync(cancellationToken);
        var usersJoinedToday = await _userRepository.GetCountJoinedTodayAsync(cancellationToken);
        var notesCreatedToday = await _noteRepository.GetCountCreatedTodayAsync(cancellationToken);

        return new AdminDashboardStatsDto
        {
            TotalUsers = totalUsers,
            TotalNotes = totalNotes,
            UsersJoinedToday = usersJoinedToday,
            NotesCreatedToday = notesCreatedToday
        };
    }

    public async Task<bool> UpdateUserRoleAsync(Guid id, string role, CancellationToken cancellationToken = default)
    {
        // Add additional validation if needed (e.g. check if the role is valid, or if the user exists).
        // The simplest implementation just passes it down.
        if (role != "User" && role != "Admin")
        {
            return false;
        }
        
        return await _userRepository.UpdateRoleAsync(id, role, cancellationToken);
    }
}
