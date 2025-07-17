using BookManagement.Application.Common.Messaging;

namespace BookManagement.Application.Users.Commands.Update;

public record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName
) : ICommand;
