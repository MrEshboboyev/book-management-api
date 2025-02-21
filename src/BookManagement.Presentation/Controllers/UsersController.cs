using BookManagement.Application.Users.Queries.GetUserById;
using BookManagement.Domain.Enums.Users;
using BookManagement.Infrastructure.Authentication;
using BookManagement.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Presentation.Controllers;

/// <summary>
/// API Controller for managing user-related operations.
/// </summary>
[ApiController]
[Route("api/users")]
[Produces("application/json")]
public sealed class UsersController(ISender sender) : ApiController(sender)
{
    /// <summary>
    /// Retrieves the details of a current user.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An IActionResult containing the user details if found, or an error message.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/users/{id}
    ///
    /// </remarks>
    /// <response code="200">Returns the user details</response>
    /// <response code="401">If the user is not authorized</response>
    [HasPermission(Permission.ReadUser)]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserById(CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(GetUserId());
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
    }
}
