using BookManagement.Application.Books.Commands.AddBook;
using BookManagement.Application.Books.Commands.AddBooksBulk;
using BookManagement.Application.Books.Commands.SoftDeleteBook;
using BookManagement.Application.Books.Commands.SoftDeleteBooksBulk;
using BookManagement.Application.Books.Commands.UpdateBook;
using BookManagement.Application.Books.Queries.GetBookById;
using BookManagement.Application.Books.Queries.GetBooks;
using BookManagement.Domain.Enums.Users;
using BookManagement.Infrastructure.Authentication;
using BookManagement.Presentation.Abstractions;
using BookManagement.Presentation.Contracts.Books;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Presentation.Controllers;

/// <summary>
/// API Controller for managing book-related operations.
/// </summary>
[Route("api/books")]
public sealed class BooksController(ISender sender) : ApiController(sender)
{
    /// <summary>
    /// Adds a new book.
    /// </summary>
    [HasPermission(Permission.AddBook)]
    [HttpPost]
    public async Task<IActionResult> AddBook(
        [FromBody] AddBookRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddBookCommand(request.Title, request.PublicationYear, request.AuthorName);
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
    }

    /// <summary>
    /// Adds multiple books in bulk.
    /// </summary>
    [HasPermission(Permission.AddBooksBulk)]
    [HttpPost("bulk")]
    public async Task<IActionResult> AddBooksBulk(
        [FromBody] AddBooksBulkRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddBooksBulkCommand([.. request.Books.Select(
            book => (book.Title, book.PublicationYear, book.AuthorName))]);
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
    }

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    [HasPermission(Permission.UpdateBook)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBook(
        Guid id,
        [FromBody] UpdateBookRequest request,
        CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Mismatched book ID.");
        }

        var command = new UpdateBookCommand(request.Id, request.Title, request.PublicationYear, request.AuthorName);
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? NoContent() : HandleFailure(response);
    }

    /// <summary>
    /// Soft deletes a book.
    /// </summary>
    [HasPermission(Permission.SoftDeleteBook)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> SoftDeleteBook(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new SoftDeleteBookCommand(id);
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? NoContent() : HandleFailure(response);
    }

    /// <summary>
    /// Soft deletes multiple books in bulk.
    /// </summary>
    [HasPermission(Permission.SoftDeleteBooksBulk)]
    [HttpDelete("bulk")]
    public async Task<IActionResult> SoftDeleteBooksBulk(
        [FromBody] SoftDeleteBooksBulkRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SoftDeleteBooksBulkCommand(request.BookIds);
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? NoContent() : HandleFailure(response);
    }

    /// <summary>
    /// Retrieves the details of a book by its unique identifier.
    /// </summary>
    [HasPermission(Permission.GetBookDetails)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBookById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookByIdQuery(id);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
    }

    /// <summary>
    /// Retrieves a list of book titles with pagination.
    /// </summary>
    [HasPermission(Permission.GetBooksList)]
    [HttpGet]
    public async Task<IActionResult> GetBooks(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetBooksQuery(pageNumber, pageSize);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
    }
}
