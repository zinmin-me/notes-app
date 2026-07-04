namespace NotesApp.Core.DTOs.Auth;

/// <summary>
/// Response model for authenticated user information.
/// </summary>
public class AuthResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public TokenResponse Token { get; set; } = new();
}
