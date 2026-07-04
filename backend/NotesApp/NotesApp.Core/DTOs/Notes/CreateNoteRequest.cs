namespace NotesApp.Core.DTOs.Notes;

/// <summary>
/// Request model for creating a new note.
/// </summary>
public class CreateNoteRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
}
