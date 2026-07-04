using NotesApp.Core.DTOs.Common;
using NotesApp.Core.DTOs.Notes;
using NotesApp.Core.Entities;
using NotesApp.Core.Interfaces.Repositories;
using NotesApp.Core.Interfaces.Services;

namespace NotesApp.Application.Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;

    public NoteService(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<NoteResponse> CreateAsync(Guid userId, CreateNoteRequest request, CancellationToken cancellationToken = default)
    {
        var note = new Note
        {
            UserId = userId,
            Title = request.Title,
            Content = request.Content
        };

        var createdNote = await _noteRepository.CreateAsync(note, cancellationToken);

        return MapToResponse(createdNote);
    }

    public async Task<PaginatedResponse<NoteResponse>> GetAllAsync(Guid userId, NoteQueryParameters parameters, CancellationToken cancellationToken = default)
    {
        var (notes, totalCount) = await _noteRepository.GetAllByUserIdAsync(userId, parameters, cancellationToken);

        var noteResponses = notes.Select(MapToResponse).ToList();

        return new PaginatedResponse<NoteResponse>(
            noteResponses,
            totalCount,
            parameters.Page,
            parameters.PageSize);
    }

    public async Task<NoteResponse> GetByIdAsync(Guid userId, Guid noteId, CancellationToken cancellationToken = default)
    {
        var note = await _noteRepository.GetByIdAndUserIdAsync(noteId, userId, cancellationToken)
            ?? throw new KeyNotFoundException($"Note with ID {noteId} not found.");

        return MapToResponse(note);
    }

    public async Task<NoteResponse> UpdateAsync(Guid userId, Guid noteId, UpdateNoteRequest request, CancellationToken cancellationToken = default)
    {
        var note = await _noteRepository.GetByIdAndUserIdAsync(noteId, userId, cancellationToken)
            ?? throw new KeyNotFoundException($"Note with ID {noteId} not found.");

        note.Title = request.Title;
        note.Content = request.Content;

        var updatedNote = await _noteRepository.UpdateAsync(note, cancellationToken);

        return MapToResponse(updatedNote);
    }

    public async Task DeleteAsync(Guid userId, Guid noteId, CancellationToken cancellationToken = default)
    {
        var deleted = await _noteRepository.DeleteAsync(noteId, userId, cancellationToken);
        if (!deleted)
            throw new KeyNotFoundException($"Note with ID {noteId} not found.");
    }

    private static NoteResponse MapToResponse(Note note)
    {
        return new NoteResponse
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }
}
