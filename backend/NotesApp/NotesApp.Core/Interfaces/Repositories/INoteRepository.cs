using NotesApp.Core.DTOs.Notes;
using NotesApp.Core.Entities;

namespace NotesApp.Core.Interfaces.Repositories;

/// <summary>
/// Repository interface for note data access operations.
/// </summary>
public interface INoteRepository
{
    Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Note?> GetByIdAndUserIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Note> Notes, int TotalCount)> GetAllByUserIdAsync(Guid userId, NoteQueryParameters parameters, CancellationToken cancellationToken = default);
    Task<Note> CreateAsync(Note note, CancellationToken cancellationToken = default);
    Task<Note> UpdateAsync(Note note, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
}
