namespace NotesApp.Core.DTOs.Auth;

/// <summary>
/// Response model containing JWT and refresh tokens.
/// </summary>
public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
