using FluentValidation;

namespace BookManagement.Application.Books.Commands.AddBook;

/// <summary>
/// Validator for the AddBookCommand.
/// </summary>
internal class AddBookCommandValidator : AbstractValidator<AddBookCommand>
{
    public AddBookCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.PublicationYear)
            .InclusiveBetween(1450, DateTime.UtcNow.Year)
            .WithMessage("Invalid publication year.");

        RuleFor(x => x.AuthorName)
            .NotEmpty().WithMessage("Author name is required.");
    }
}
