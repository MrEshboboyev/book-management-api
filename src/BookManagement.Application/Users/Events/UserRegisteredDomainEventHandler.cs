using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Events.Users;
using BookManagement.Domain.Repositories.Users;

namespace BookManagement.Application.Users.Events;

internal sealed class UserRegisteredDomainEventHandler(
    IUserRepository userRepository)
          : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(
        UserRegisteredDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(
            notification.UserId,
            cancellationToken);

        if (user is null)
        {
            return;
        }

        Console.WriteLine($"User registered with email : {user.Email.Value}");
    }
}
