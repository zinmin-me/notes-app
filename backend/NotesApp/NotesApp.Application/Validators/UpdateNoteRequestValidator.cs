using FluentValidation;
using NotesApp.Core.DTOs.Notes;

namespace NotesApp.Application.Validators;

public class UpdateNoteRequestValidator : AbstractValidator<UpdateNoteRequest>
{
    public UpdateNoteRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Content)
            .MaximumLength(100000).WithMessage("Content is too long.");
    }
}
