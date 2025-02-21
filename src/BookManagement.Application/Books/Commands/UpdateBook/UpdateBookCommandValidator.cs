using FluentValidation;

namespace BookManagement.Application.Books.Commands.UpdateBook;

/// <summary>
/// Validator for UpdateBookCommand.
/// </summary>
internal class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.PublicationYear)
            .GreaterThan(0).WithMessage("Publication year must be a positive number.");

        RuleFor(x => x.AuthorName)
            .NotEmpty().WithMessage("Author name is required.")
            .MaximumLength(100).WithMessage("Author name must not exceed 100 characters.");
    }
}
