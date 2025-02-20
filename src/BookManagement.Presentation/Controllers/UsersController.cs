using BookManagement.Application.Users.Queries.GetUserById;
using BookManagement.Domain.Enums.Users;
using BookManagement.Infrastructure.Authentication;
using BookManagement.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Presentation.Controllers;

/// <summary>
/// API Controller for managing user-related operations.
/// </summary>
[Route("api/users")]
public sealed class UsersController(ISender sender) : ApiController(sender)
{
    /// <summary>
    /// Retrieves the details of a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An IActionResult containing the user details if found, or an error message.</returns>
    [HasPermission(Permission.ReadUser)]
    [HttpGet]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(GetUserId());

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
}