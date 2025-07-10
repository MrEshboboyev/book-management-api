using BookManagement.Application.Books.Commands.SoftDeleteBook;
using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Repositories;
using Moq;
using BookManagement.Application.UnitTests.Common;

namespace BookManagement.Application.UnitTests.Books.Commands;

public class SoftDeleteBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly SoftDeleteBookCommandHandler _handler;

    public SoftDeleteBookCommandHandlerTests()
    {
        _handler = new SoftDeleteBookCommandHandler(_bookRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_When_BookIsSoftDeleted()
    {
        // Arrange
        var book = Helpers.CreateTestBook("Sample Book",
                                          2023,
                                          "John Doe");

        _bookRepositoryMock
            .Setup(repo => repo.GetByIdAsync(book.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        var command = new SoftDeleteBookCommand(book.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _bookRepositoryMock.Verify(repo => repo.UpdateAsync(book, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_BookNotFound()
    {
        // Arrange
        var bookId = Guid.NewGuid();

        _bookRepositoryMock
            .Setup(repo => repo.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book)null!);

        var command = new SoftDeleteBookCommand(bookId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Book.NotFound(bookId), result.Error);
    }
}