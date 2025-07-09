using BookManagement.Application.Users.Commands.Login;
using BookManagement.Application.Users.Commands.RegisterUser;
using BookManagement.Presentation.Abstractions;
using BookManagement.Presentation.Contracts.Users;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Presentation.Controllers;

/// <summary>
/// API Controller for managing auth-related operations.
/// </summary>
[ApiController]
[Route("api/auth")]
[ServiceFilter(typeof(MediatorActionFilter))]
public sealed class AuthController : ControllerBase
{
    [HttpPost("login")]
    [MediatorEndpoint(typeof(LoginCommand), typeof(string))]
    public void Login([FromBody] LoginRequest request) { }

    [HttpPost("register")]
    [MediatorEndpoint(typeof(RegisterUserCommand), typeof(Guid))]
    public void Register([FromBody] RegisterUserRequest request) { }
}
