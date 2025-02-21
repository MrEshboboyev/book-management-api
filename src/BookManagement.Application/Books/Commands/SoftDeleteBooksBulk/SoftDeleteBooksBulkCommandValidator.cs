using FluentValidation;

namespace BookManagement.Application.Books.Commands.SoftDeleteBooksBulk;

/// <summary>
/// Validator for SoftDeleteBooksBulkCommand.
/// </summary>
internal class SoftDeleteBooksBulkCommandValidator : AbstractValidator<SoftDeleteBooksBulkCommand>
{
    public SoftDeleteBooksBulkCommandValidator()
    {
        RuleFor(x => x.BookIds)
            .NotEmpty().WithMessage("BookIds list cannot be empty.")
            .Must(bookIds => bookIds.All(id => id != Guid.Empty))
            .WithMessage("All BookIds must be valid GUIDs.");
    }
}
