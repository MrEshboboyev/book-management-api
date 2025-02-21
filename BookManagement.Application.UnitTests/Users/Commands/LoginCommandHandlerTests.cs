using BookManagement.Application.Abstractions.Security;
using BookManagement.Application.UnitTests.Common;
using BookManagement.Application.Users.Commands.Login;
using BookManagement.Domain.Entities.Users;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.ValueObjects.Users;
using Moq;

namespace BookManagement.Application.UnitTests.Users.Commands;

public class LoginCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IJwtProvider> _jwtProviderMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();

    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _handler = new LoginCommandHandler(
            _userRepositoryMock.Object,
            _jwtProviderMock.Object,
            _passwordHasherMock.Object);
    }
    
    #endregion

    #region Test Methods
    
    [Fact]
    public async Task Handle_Should_LoginSuccessfully_When_CredentialsAreValid()
    {
        // Arrange
        var command = new LoginCommand(
            Email: "user@example.com",
            Password: "securePassword");

        var userId = Guid.NewGuid();
        var hashedPassword = "hashedPassword";
        var token = "generated-jwt-token";

        var user = Helpers.CreateTestUser(
            id: userId,
            email: command.Email,
            passwordHash: hashedPassword,
            firstName: "John",
            lastName: "Doe");

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(hasher => hasher.Verify(command.Password, hashedPassword))
            .Returns(true);

        _jwtProviderMock
            .Setup(provider => provider.GenerateAsync(user))
            .ReturnsAsync(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(token, result.Value);
        _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()), Times.Once);
        _passwordHasherMock.Verify(hasher => hasher.Verify(command.Password, hashedPassword), Times.Once);
        _jwtProviderMock.Verify(provider => provider.GenerateAsync(user), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_UserDoesNotExist()
    {
        // Arrange
        var command = new LoginCommand(
            Email: "user@example.com",
            Password: "securePassword");

        var emailObj = Email.Create(command.Email).Value;

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(emailObj, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.InvalidCredentials, result.Error);
        _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(emailObj, It.IsAny<CancellationToken>()), Times.Once);
        _passwordHasherMock.Verify(hasher => hasher.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _jwtProviderMock.Verify(provider => provider.GenerateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_PasswordDoesNotMatch()
    {
        // Arrange
        var command = new LoginCommand(
            Email: "user@example.com",
            Password: "securePassword");

        var userId = Guid.NewGuid();
        var hashedPassword = "hashedPassword";

        var user = Helpers.CreateTestUser(
            id: userId,
            email: command.Email,
            passwordHash: hashedPassword,
            firstName: "John",
            lastName: "Doe");

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(hasher => hasher.Verify(command.Password, hashedPassword))
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.InvalidCredentials, result.Error);
        _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()), Times.Once);
        _passwordHasherMock.Verify(hasher => hasher.Verify(command.Password, hashedPassword), Times.Once);
        _jwtProviderMock.Verify(provider => provider.GenerateAsync(It.IsAny<User>()), Times.Never);
    }
    
    #endregion
}