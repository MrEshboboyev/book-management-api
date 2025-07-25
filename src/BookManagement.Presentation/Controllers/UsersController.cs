﻿using BookManagement.Application.Common.Pagination;
using BookManagement.Application.Users.Commands.Update;
using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Application.Users.Queries.GetAllUsers;
using BookManagement.Application.Users.Queries.GetUserById;
using BookManagement.Application.Users.Queries.GetUserWithRolesById;
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
    //[HasPermission(Permission.ReadUser)]
    [MediatorEndpoint(typeof(GetAllUsersQuery), typeof(PaginatedList<UserResponse>))]
    public void GetAllUsers([FromQuery] GetAllUsersQuery query) { }

    [HttpGet("{userId:guid}")]
    [HasPermission(Permission.ReadUser)]
    [MediatorEndpoint(typeof(GetUserByIdQuery), typeof(UserResponse))]
    public void Get(Guid userId) { }

    [HttpGet("{userId:guid}/with-roles")]
    [HasPermission(Permission.ReadUser)]
    [MediatorEndpoint(typeof(GetUserWithRolesByIdQuery), typeof(UserWithRolesResponse))]
    public void GetUserWithRolesById(Guid userId) { }

    [HttpPut("{userId:guid}")]
    [HasPermission(Permission.UpdateUser)]
    [MediatorEndpoint(typeof(UpdateUserCommand), typeof(void))]
    public void Update(
        Guid userId,
        [FromBody] UpdateUserRequest request)
    { }
}
