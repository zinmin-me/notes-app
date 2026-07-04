namespace NotesApp.Core.Helpers;

/// <summary>
/// BCrypt password hashing utility.
/// </summary>
public static class PasswordHasher
{
    private const int WorkFactor = 11;

    /// <summary>
    /// Hashes a plain-text password using BCrypt.
    /// </summary>
    public static string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    /// <summary>
    /// Verifies a plain-text password against a BCrypt hash.
    /// </summary>
    public static bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
