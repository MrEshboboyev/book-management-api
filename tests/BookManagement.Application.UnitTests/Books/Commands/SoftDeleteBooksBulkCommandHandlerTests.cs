using BookManagement.Application.Books.Commands.SoftDeleteBooksBulk;
using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Repositories;
using Moq;
using BookManagement.Application.UnitTests.Common;

namespace BookManagement.Application.UnitTests.Books.Commands;

public class SoftDeleteBooksBulkCommandHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly SoftDeleteBooksBulkCommandHandler _handler;

    public SoftDeleteBooksBulkCommandHandlerTests()
    {
        _handler = new SoftDeleteBooksBulkCommandHandler(_bookRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_When_BooksAreSoftDeleted()
    {
        // Arrange
        var books = new List<Book>()
        {
            Helpers.CreateTestBook("Book1",
                                    2024,
                                    "John Doe"),
            Helpers.CreateTestBook("Book2",
                                    2025,
                                    "John Doe"),
        };
        // Mock GetByIdAsync to return the correct book based on the provided ID
        _bookRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid id, CancellationToken _) => books.First(b => b.Id == id));

        var command = new SoftDeleteBooksBulkCommand([.. books.Select(b => b.Id)]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _bookRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Exactly(books.Count));
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}