namespace NotesApp.Core.Entities;

/// <summary>
/// Represents a refresh token for JWT token rotation.
/// </summary>
public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public DateTime? Revoked { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Indicates whether the token has expired.
    /// </summary>
    public bool IsExpired => DateTime.UtcNow >= Expires;

    /// <summary>
    /// Indicates whether the token has been revoked.
    /// </summary>
    public bool IsRevoked => Revoked.HasValue;

    /// <summary>
    /// Indicates whether the token is still active (not expired and not revoked).
    /// </summary>
    public bool IsActive => !IsExpired && !IsRevoked;
}
