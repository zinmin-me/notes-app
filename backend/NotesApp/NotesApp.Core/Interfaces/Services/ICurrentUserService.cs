namespace NotesApp.Core.Interfaces.Services;

/// <summary>
/// Service interface for accessing the current authenticated user's information.
/// </summary>
public interface ICurrentUserService
{
    Guid UserId { get; }
    string Email { get; }
    string Role { get; }
    bool IsAuthenticated { get; }
}
