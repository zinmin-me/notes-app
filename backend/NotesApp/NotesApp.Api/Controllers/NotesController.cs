using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Core.DTOs.Common;
using NotesApp.Core.DTOs.Notes;
using NotesApp.Core.Interfaces.Services;

namespace NotesApp.Api.Controllers;

/// <summary>
/// Controller for managing user notes.
/// </summary>
[Authorize]
public class NotesController : BaseApiController
{
    private readonly INoteService _noteService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IValidator<CreateNoteRequest> _createValidator;
    private readonly IValidator<UpdateNoteRequest> _updateValidator;

    public NotesController(
        INoteService noteService,
        ICurrentUserService currentUserService,
        IValidator<CreateNoteRequest> createValidator,
        IValidator<UpdateNoteRequest> updateValidator)
    {
        _noteService = noteService;
        _currentUserService = currentUserService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Gets a paginated list of notes for the current user.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<NoteResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] NoteQueryParameters parameters, CancellationToken cancellationToken)
    {
        var response = await _noteService.GetAllAsync(_currentUserService.UserId, parameters, cancellationToken);
        return Ok(ApiResponse<PaginatedResponse<NoteResponse>>.SuccessResponse(response));
    }

    /// <summary>
    /// Gets a specific note by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<NoteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _noteService.GetByIdAsync(_currentUserService.UserId, id, cancellationToken);
        return Ok(ApiResponse<NoteResponse>.SuccessResponse(response));
    }

    /// <summary>
    /// Creates a new note.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<NoteResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var response = await _noteService.CreateAsync(_currentUserService.UserId, request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, ApiResponse<NoteResponse>.SuccessResponse(response, "Note created successfully."));
    }

    /// <summary>
    /// Updates an existing note.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<NoteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNoteRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var response = await _noteService.UpdateAsync(_currentUserService.UserId, id, request, cancellationToken);
        return Ok(ApiResponse<NoteResponse>.SuccessResponse(response, "Note updated successfully."));
    }

    /// <summary>
    /// Deletes a note by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _noteService.DeleteAsync(_currentUserService.UserId, id, cancellationToken);
        return Ok(ApiResponse.SuccessResponse("Note deleted successfully."));
    }
}
