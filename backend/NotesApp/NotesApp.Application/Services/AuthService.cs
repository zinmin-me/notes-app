using NotesApp.Core.DTOs.Auth;
using NotesApp.Core.Entities;
using NotesApp.Core.Enums;
using NotesApp.Core.Helpers;
using NotesApp.Core.Interfaces.Repositories;
using NotesApp.Core.Interfaces.Services;

namespace NotesApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.EmailExistsAsync(request.Email, cancellationToken))
            throw new InvalidOperationException("Email is already in use.");

        if (await _userRepository.UsernameExistsAsync(request.Username, cancellationToken))
            throw new InvalidOperationException("Username is already taken.");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = PasswordHasher.Hash(request.Password),
            Role = UserRole.User.ToString()
        };

        var createdUser = await _userRepository.CreateAsync(user, cancellationToken);
        var tokenResponse = await GenerateTokensAsync(createdUser, cancellationToken);

        return new AuthResponse
        {
            Id = createdUser.Id,
            Username = createdUser.Username,
            Email = createdUser.Email,
            Role = createdUser.Role,
            Token = tokenResponse
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var tokenResponse = await GenerateTokensAsync(user, cancellationToken);

        return new AuthResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Token = tokenResponse
        };
    }

    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        if (!storedToken.IsActive)
            throw new UnauthorizedAccessException("Refresh token is expired or revoked.");

        var user = await _userRepository.GetByIdAsync(storedToken.UserId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found.");

        // Revoke the old token (token rotation)
        await _refreshTokenRepository.RevokeAsync(storedToken.Token, cancellationToken);

        // Generate new tokens
        return await GenerateTokensAsync(user, cancellationToken);
    }

    public async Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        await _refreshTokenRepository.RevokeAsync(refreshToken, cancellationToken);
    }

    public async Task<UserProfileResponse> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found.");

        return new UserProfileResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }

    private async Task<TokenResponse> GenerateTokensAsync(User user, CancellationToken cancellationToken)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var expiry = _tokenService.GetRefreshTokenExpiry();

        await _refreshTokenRepository.CreateAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            Expires = expiry
        }, cancellationToken);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiry
        };
    }
}
