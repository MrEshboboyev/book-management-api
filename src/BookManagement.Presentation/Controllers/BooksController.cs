using BookManagement.Application.Books.Commands.AddBook;
using BookManagement.Application.Books.Commands.AddBooksBulk;
using BookManagement.Application.Books.Commands.SoftDeleteBook;
using BookManagement.Application.Books.Commands.SoftDeleteBooksBulk;
using BookManagement.Application.Books.Commands.UpdateBook;
using BookManagement.Application.Books.Queries.GetBookById;
using BookManagement.Application.Books.Queries.GetBooks;
using BookManagement.Domain.Enums.Users;
using BookManagement.Domain.Identity.Books;
using BookManagement.Infrastructure.Authentication;
using BookManagement.Presentation.Abstractions;
using BookManagement.Presentation.Contracts.Books;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Presentation.Controllers;

/// <summary>
/// API Controller for managing book-related operations.
/// </summary>
[ApiController]
[Route("api/books")]
[Produces("application/json")]
public sealed class BooksController(ISender sender) : ApiController(sender)
{
    #region Commands

    /// <summary>
    /// Adds a new book.
    /// </summary>
    /// <param name="request">The request containing book details.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The newly created book's ID.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/books
    ///     {
    ///        "title": "Sample Book Title",
    ///        "publicationYear": 2023,
    ///        "authorName": "Author Name"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created book's ID</response>
    /// <response code="400">If the request is invalid</response>
    [HasPermission(Permission.AddBook)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddBook(
        [FromBody] AddBookRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddBookCommand(request.Title, request.PublicationYear, request.AuthorName);
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? CreatedAtAction(nameof(GetBookById), new { id = response.Value }, response.Value) : HandleFailure(response);
    }

    /// <summary>
    /// Adds multiple books in bulk.
    /// </summary>
    /// <param name="request">The request containing multiple book details.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The list of newly created book IDs.</returns>
    /// <response code="201">Returns the list of newly created book IDs</response>
    /// <response code="400">If the request is invalid</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/books/bulk
    ///     {
    ///        "books": [
    ///            {
    ///                "title": "Sample Book Title 1",
    ///                "publicationYear": 2023,
    ///                "authorName": "Author Name 1"
    ///            },
    ///            {
    ///                "title": "Sample Book Title 2",
    ///                "publicationYear": 2022,
    ///                "authorName": "Author Name 2"
    ///            }
    ///        ]
    ///     }
    ///
    /// </remarks>
    [HasPermission(Permission.AddBooksBulk)]
    [HttpPost("bulk")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddBooksBulk(
        [FromBody] AddBooksBulkRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddBooksBulkCommand(request.Books.Select(book => (book.Title, book.PublicationYear, book.AuthorName)).ToList());
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? CreatedAtAction(nameof(GetBooks), response.Value) : HandleFailure(response);
    }

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    /// <param name="id">The ID of the book to update.</param>
    /// <param name="request">The request containing updated book details.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>No content if the update is successful.</returns>
    /// <response code="204">If the update is successful</response>
    /// <response code="400">If the request is invalid</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/books/{id}
    ///     {
    ///        "id": "1",
    ///        "title": "Updated Book Title",
    ///        "publicationYear": 2023,
    ///        "authorName": "Updated Author Name"
    ///     }
    ///
    /// </remarks>
    [HasPermission(Permission.UpdateBook)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBook(
        BookId id,
        [FromBody] UpdateBookRequest request,
        CancellationToken cancellationToken)
    {
        if (!id.Equals(request.Id))
            return BadRequest("Mismatched book ID.");

        var command = new UpdateBookCommand(
            request.Id,
            request.Title,
            request.PublicationYear,
            request.AuthorName);

        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess
            ? NoContent()
            : HandleFailure(response);
    }


    /// <summary>
    /// Soft deletes a book.
    /// </summary>
    /// <param name="id">The ID of the book to delete.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <response code="204">If the deletion is successful</response>
    /// <response code="400">If the request is invalid</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/books/{id}
    ///
    /// </remarks>
    [HasPermission(Permission.SoftDeleteBook)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SoftDeleteBook(
        BookId id,
        CancellationToken cancellationToken)
    {
        var command = new SoftDeleteBookCommand(id);
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? NoContent() : HandleFailure(response);
    }

    /// <summary>
    /// Soft deletes multiple books in bulk.
    /// </summary>
    /// <param name="request">The request containing the list of book IDs to delete.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <response code="204">If the deletion is successful</response>
    /// <response code="400">If the request is invalid</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/books/bulk
    ///     {
    ///        "bookIds": [
    ///            "00000000-0000-0000-0000-000000000000",
    ///            "11111111-1111-1111-1111-111111111111"
    ///        ]
    ///     }
    ///
    /// </remarks>
    [HasPermission(Permission.SoftDeleteBooksBulk)]
    [HttpDelete("bulk")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SoftDeleteBooksBulk(
        [FromBody] SoftDeleteBooksBulkRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SoftDeleteBooksBulkCommand(request.BookIds);
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? NoContent() : HandleFailure(response);
    }

    #endregion

    #region Queries

    /// <summary>
    /// Retrieves the details of a book by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the book to retrieve.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The details of the book.</returns>
    /// <response code="200">Returns the book details</response>
    /// <response code="404">If the book is not found</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/books/{id}
    ///
    /// </remarks>
    [HasPermission(Permission.GetBookDetails)]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBookById(
        BookId id, 
        CancellationToken cancellationToken)
    {
        var query = new GetBookByIdQuery(id);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
    }

    /// <summary>
    /// Retrieves a list of book titles with pagination.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A paginated list of book titles.</returns>
    /// <response code="200">Returns the paginated list of book titles</response>
    /// <response code="400">If the request is invalid</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/books?pageNumber=1&pageSize=10
    ///
    /// </remarks>
    [HasPermission(Permission.GetBooksList)]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBooks(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetBooksQuery(pageNumber, pageSize);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
    }

    #endregion
}
