using FluentValidation;

namespace BookManagement.Application.Books.Commands.AddBooksBulk;


/// <summary>
/// Validator for AddBooksBulkCommand.
/// </summary>
internal class AddBooksBulkCommandValidator : AbstractValidator<AddBooksBulkCommand>
{
    public AddBooksBulkCommandValidator()
    {
        RuleForEach(x => x.Books).ChildRules(books =>
        {
            books.RuleFor(book => book.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            books.RuleFor(book => book.PublicationYear)
                .GreaterThan(0).WithMessage("Publication year must be a positive number.");

            books.RuleFor(book => book.AuthorName)
                .NotEmpty().WithMessage("Author name is required.")
                .MaximumLength(100).WithMessage("Author name must not exceed 100 characters.");
        });
    }
}