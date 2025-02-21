using BookManagement.Application.Books.Commands.AddBooksBulk;
using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.ValueObjects.Books;
using Moq;

namespace BookManagement.Application.UnitTests.Books.Commands;

public class AddBooksBulkCommandHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly AddBooksBulkCommandHandler _handler;

    public AddBooksBulkCommandHandlerTests()
    {
        _handler = new AddBooksBulkCommandHandler(_bookRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_When_BooksAreAdded()
    {
        // Arrange
        var books = new List<(string Title, int PublicationYear, string AuthorName)>
        {
            ("Book 1", 2023, "Author 1"),
            ("Book 2", 2022, "Author 2")
        };

        var command = new AddBooksBulkCommand(books);

        _bookRepositoryMock
            .Setup(repo => repo.ExistsByTitleAsync(It.IsAny<Title>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(books.Count, result.Value.Count);
        _bookRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Exactly(books.Count));
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}