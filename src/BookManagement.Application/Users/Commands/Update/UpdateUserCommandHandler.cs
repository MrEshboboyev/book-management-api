using BookManagement.Application.Common.Messaging;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.Shared;

namespace BookManagement.Application.Users.Commands.Update;

internal sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        // fetch existing user
        var user = await userRepository
            .GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(
                DomainErrors.User.NotFound(request.UserId));
        }

        // apply updates
        user.UpdateName(request.FirstName, request.LastName);

        // persist changes
        await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}