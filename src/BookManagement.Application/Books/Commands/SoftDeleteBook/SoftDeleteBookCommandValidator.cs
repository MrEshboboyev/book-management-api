using FluentValidation;

namespace BookManagement.Application.Books.Commands.SoftDeleteBook;

internal class SoftDeleteBookCommandValidator : AbstractValidator<SoftDeleteBookCommand>
{
    public SoftDeleteBookCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id must not be empty.");
    }
}
