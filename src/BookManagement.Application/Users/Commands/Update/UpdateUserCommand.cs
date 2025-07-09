using BookManagement.Application.Abstractions.Messaging;

namespace BookManagement.Application.Users.Commands.Update;

public record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName
) : ICommand;
