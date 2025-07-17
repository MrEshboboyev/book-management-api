using BookManagement.Application.Common.Security;
using BookManagement.Application.Users.Commands.RegisterUser;
using BookManagement.Domain.Entities.Users;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.ValueObjects.Users;
using Moq;

namespace BookManagement.Application.UnitTests.Users.Commands;

public class RegisterUserCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IRoleRepository> _roleRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _handler = new RegisterUserCommandHandler(
            _userRepositoryMock.Object,
            _roleRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object);
    }
    
    #endregion
    
    #region Test Methods

    [Fact]
    public async Task Handle_Should_RegisterUser_Successfully()
    {
        // Arrange
        var command = new RegisterUserCommand(
            Email: "user@example.com",
            Password: "securePassword",
            FirstName: "John",
            LastName: "Doe");

        var email = Email.Create(command.Email).Value;

        var hashedPassword = "hashed-password";

        _userRepositoryMock
            .Setup(repo => repo.IsEmailUniqueAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _roleRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Role.Registered);

        _passwordHasherMock
            .Setup(hasher => hasher.Hash(command.Password))
            .Returns(hashedPassword);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_If_Email_Is_Not_Unique()
    {
        // Arrange
        var command = new RegisterUserCommand(
            Email: "user@example.com",
            Password: "securePassword",
            FirstName: "John",
            LastName: "Doe");

        var email = Email.Create(command.Email).Value;

        _userRepositoryMock
            .Setup(repo => repo.IsEmailUniqueAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.EmailAlreadyInUse, result.Error);
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_EmailIsInvalid()
    {
        // Arrange
        var command = new RegisterUserCommand(
            Email: "invalid-email",
            Password: "securePassword",
            FirstName: "John",
            LastName: "Doe");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Email.InvalidFormat, result.Error);
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_FirstNameIsInvalid()
    {
        // Arrange
        var command = new RegisterUserCommand(
            Email: "user@example.com",
            Password: "securePassword",
            FirstName: "",
            LastName: "Doe");
        
        var email = Email.Create(command.Email).Value;
        
        _userRepositoryMock
            .Setup(repo => repo.IsEmailUniqueAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.FirstName.Empty, result.Error);
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_LastNameIsInvalid()
    {
        // Arrange
        var command = new RegisterUserCommand(
            Email: "user@example.com",
            Password: "securePassword",
            FirstName: "John",
            LastName: "");
        
        var email = Email.Create(command.Email).Value;
        
        _userRepositoryMock
            .Setup(repo => repo.IsEmailUniqueAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.LastName.Empty, result.Error);
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    #endregion
}