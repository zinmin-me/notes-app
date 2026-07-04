namespace NotesApp.Core.Entities;

/// <summary>
/// Represents a note created by a user.
/// </summary>
public class Note
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
