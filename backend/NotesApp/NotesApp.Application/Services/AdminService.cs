using NotesApp.Core.DTOs.Auth;
using NotesApp.Core.Interfaces.Repositories;
using NotesApp.Core.Interfaces.Services;

namespace NotesApp.Application.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;

    public AdminService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
}
