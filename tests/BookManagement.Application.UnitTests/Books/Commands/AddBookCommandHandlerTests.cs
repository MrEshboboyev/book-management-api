using BookManagement.Application.Books.Commands.AddBook;
using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.ValueObjects.Books;
using Moq;

namespace BookManagement.Application.UnitTests.Books.Commands;

public class AddBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly AddBookCommandHandler _handler;

    public AddBookCommandHandlerTests()
    {
        _handler = new AddBookCommandHandler(_bookRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_When_BookIsAdded()
    {
        // Arrange
        var command = new AddBookCommand(
            Title: "Sample Book",
            PublicationYear: 2023,
            AuthorName: "John Doe");

        var titleResult = Title.Create(command.Title);
        var authorResult = Author.Create(command.AuthorName);
        var publicationYearResult = PublicationYear.Create(command.PublicationYear);

        _bookRepositoryMock
            .Setup(repo => repo.ExistsByTitleAsync(titleResult.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _bookRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_BookAlreadyExists()
    {
        // Arrange
        var command = new AddBookCommand(
            Title: "Sample Book",
            PublicationYear: 2023,
            AuthorName: "John Doe");

        var titleResult = Title.Create(command.Title);

        _bookRepositoryMock
            .Setup(repo => repo.ExistsByTitleAsync(titleResult.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Book.AlreadyExists(command.Title), result.Error);
    }
}