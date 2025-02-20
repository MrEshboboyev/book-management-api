using FluentValidation;

namespace BookManagement.Application.Users.Commands.Login;

/// <summary>
/// Validator for the LoginCommand.
/// Ensures that all fields of the command are valid.
/// </summary>
internal class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    /// <summary>
    /// Initializes a new instance of the LoginCommandValidator class.
    /// Defines validation rules for the LoginCommand.
    /// </summary>
    public LoginCommandValidator()
    {
        // Ensure the Email field is not empty and follows a valid email format
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        // Ensure the Password field is not empty and is not null
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .NotNull()
            .WithMessage("Password cannot be null.");
    }
}