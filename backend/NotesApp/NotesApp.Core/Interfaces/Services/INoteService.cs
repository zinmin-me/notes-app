using NotesApp.Core.DTOs.Common;
using NotesApp.Core.DTOs.Notes;

namespace NotesApp.Core.Interfaces.Services;

/// <summary>
/// Service interface for note business operations.
/// </summary>
public interface INoteService
{
    Task<NoteResponse> CreateAsync(Guid userId, CreateNoteRequest request, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<NoteResponse>> GetAllAsync(Guid userId, NoteQueryParameters parameters, CancellationToken cancellationToken = default);
    Task<NoteResponse> GetByIdAsync(Guid userId, Guid noteId, CancellationToken cancellationToken = default);
    Task<NoteResponse> UpdateAsync(Guid userId, Guid noteId, UpdateNoteRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid userId, Guid noteId, CancellationToken cancellationToken = default);
}
