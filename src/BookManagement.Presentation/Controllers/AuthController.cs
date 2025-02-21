using BookManagement.Application.Users.Commands.RegisterUser;
using BookManagement.Application.Users.Commands.Login;
using BookManagement.Presentation.Abstractions;
using BookManagement.Presentation.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Presentation.Controllers;

/// <summary>
/// API Controller for managing auth-related operations.
/// </summary>
[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public sealed class AuthController(ISender sender) : ApiController(sender)
{
    /// <summary>
    /// Logs in a user by validating their credentials and generating a token.
    /// </summary>
    /// <param name="request">The login request containing the user's email and password.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An IActionResult containing the token if successful, or an error message.</returns>
    /// <response code="200">Returns the token if login is successful</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="401">If the credentials are invalid</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/auth/login
    ///     {
    ///        "email": "user@example.com",
    ///        "password": "password123"
    ///     }
    ///
    /// </remarks>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);

        var tokenResult = await Sender.Send(command, cancellationToken);

        return tokenResult.IsFailure ? HandleFailure(tokenResult) : Ok(tokenResult.Value);
    }

    /// <summary>
    /// Registers a new user by creating their account with the provided details.
    /// </summary>
    /// <param name="request">The registration request containing the user's details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An IActionResult containing the new user's ID if successful, or an error message.</returns>
    /// <response code="201">Returns the newly registered user's ID</response>
    /// <response code="400">If the request is invalid</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/auth/register
    ///     {
    ///        "email": "user@example.com",
    ///        "password": "password123",
    ///        "firstName": "John",
    ///        "lastName": "Doe"
    ///     }
    ///
    /// </remarks>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName);

        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(UsersController.GetUserById), "Users", new { id = result.Value }, result.Value);
    }
}
