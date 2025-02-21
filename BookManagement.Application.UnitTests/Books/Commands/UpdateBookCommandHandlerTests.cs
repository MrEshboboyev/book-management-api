using BookManagement.Application.Books.Commands.UpdateBook;
using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.ValueObjects.Books;
using Moq;
using BookManagement.Application.UnitTests.Common;

namespace BookManagement.Application.UnitTests.Books.Commands;

public class UpdateBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly UpdateBookCommandHandler _handler;

    public UpdateBookCommandHandlerTests()
    {
        _handler = new UpdateBookCommandHandler(_bookRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_When_BookIsUpdated()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var book = Helpers.CreateTestBook(bookId, "Old Title", 2022, "Old Author");

        var command = new UpdateBookCommand(
            Id: bookId,
            Title: "New Title",
            PublicationYear: 2023,
            AuthorName: "New Author");

        _bookRepositoryMock
            .Setup(repo => repo.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        _bookRepositoryMock
            .Setup(repo => repo.ExistsByTitleAsync(It.IsAny<Title>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _bookRepositoryMock.Verify(repo => repo.UpdateAsync(book, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}