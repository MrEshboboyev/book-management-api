using BookManagement.Application.Users.Commands.Update;
using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Application.Users.Queries.GetUserById;
using BookManagement.Domain.Enums.Users;
using BookManagement.Infrastructure.Authentication;
using BookManagement.Presentation.Abstractions;
using BookManagement.Presentation.Contracts.Users;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Presentation.Controllers;

/// <summary>
/// API Controller for managing user-related operations.
/// </summary>
// Ultra‐simplified controller
[ApiController]
[Route("api/users")]
[ServiceFilter(typeof(MediatorActionFilter))]
public sealed class UsersController : ControllerBase
{
    [HttpGet]
    [HasPermission(Permission.ReadUser)]
    [MediatorEndpoint(typeof(GetUserByIdQuery), typeof(UserResponse))]
    public void Get() { }

    [HttpPut("{userId:guid}")]
    [HasPermission(Permission.UpdateUser)]
    [MediatorEndpoint(typeof(UpdateUserCommand), typeof(void))]
    public void Update(
        Guid userId,
        [FromBody] UpdateUserRequest request)
    { }
}
