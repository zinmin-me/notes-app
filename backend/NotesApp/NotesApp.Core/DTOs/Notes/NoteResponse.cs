namespace NotesApp.Core.DTOs.Notes;

/// <summary>
/// Response model for a note.
/// </summary>
public class NoteResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
