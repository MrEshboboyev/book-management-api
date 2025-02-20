using FluentValidation;
using BookManagement.Domain.ValueObjects.Users;

namespace BookManagement.Application.Users.Commands.RegisterUser;

/// <summary>
/// Validator for the RegisterUserCommand.
/// Ensures that all fields of the command are valid.
/// </summary>
internal class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the RegisterUserCommandValidator class.
    /// Defines validation rules for the RegisterUserCommand.
    /// </summary>
    public RegisterUserCommandValidator()
    {
        // Ensure the Email field is not empty
        RuleFor(x => x.Email).NotEmpty()
            .WithMessage("Email is required.");

        // Ensure the FirstName field is not empty and does not exceed the maximum length
        RuleFor(x => x.FirstName).NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(FirstName.MaxLength)
            .WithMessage($"First name cannot exceed {FirstName.MaxLength} characters.");

        // Ensure the LastName field is not empty and does not exceed the maximum length
        RuleFor(x => x.LastName).NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(LastName.MaxLength)
            .WithMessage($"Last name cannot exceed {LastName.MaxLength} characters.");
    }
}