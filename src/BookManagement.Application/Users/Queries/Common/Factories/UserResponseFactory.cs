using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Domain.Entities.Users;

namespace BookManagement.Application.Users.Queries.Common.Factories;

public static class UserResponseFactory
{
    public static UserResponse Create(User user)
    {
        return new UserResponse(
            user.Id,
            user.Email.Value,
            user.FirstName.Value,
            user.LastName.Value);
    }
}