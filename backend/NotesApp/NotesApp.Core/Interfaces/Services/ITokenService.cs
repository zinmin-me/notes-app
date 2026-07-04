using NotesApp.Core.Entities;

namespace NotesApp.Core.Interfaces.Services;

/// <summary>
/// Service interface for JWT token generation and validation.
/// </summary>
public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpiry();
}
