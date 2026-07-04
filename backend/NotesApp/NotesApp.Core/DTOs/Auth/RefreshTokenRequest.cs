namespace NotesApp.Core.DTOs.Auth;

/// <summary>
/// Request model for refreshing an access token.
/// </summary>
public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
