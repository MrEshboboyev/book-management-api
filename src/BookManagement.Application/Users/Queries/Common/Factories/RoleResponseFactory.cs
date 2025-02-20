using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Domain.Entities.Users;

namespace BookManagement.Application.Users.Queries.Common.Factories;

public static class RoleResponseFactory
{
    public static RoleResponse Create(Role role)
    {
        return new RoleResponse(
            role.Id,
            role.Name);
    }
}