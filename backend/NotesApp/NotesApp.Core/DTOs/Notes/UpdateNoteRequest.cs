namespace NotesApp.Core.DTOs.Notes;

/// <summary>
/// Request model for updating an existing note.
/// </summary>
public class UpdateNoteRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
}
