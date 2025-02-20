using BookManagement.Application.Books.Commands.AddBook;
using BookManagement.Application.Books.Commands.AddBooksBulk;
using BookManagement.Application.Books.Commands.SoftDeleteBook;
using BookManagement.Application.Books.Commands.SoftDeleteBooksBulk;
using BookManagement.Application.Books.Commands.UpdateBook;
using BookManagement.Application.Books.Queries.GetBookById;
using BookManagement.Application.Books.Queries.GetBooks;
using BookManagement.Presentation.Abstractions;
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
    [HttpPost]
    public async Task<IActionResult> AddBook(
        [FromBody] AddBookCommand command, 
        CancellationToken cancellationToken)
    {
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
    }

    /// <summary>
    /// Adds multiple books in bulk.
    /// </summary>
    [HttpPost("bulk")]
    public async Task<IActionResult> AddBooksBulk(
        [FromBody] AddBooksBulkCommand command, 
        CancellationToken cancellationToken)
    {
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
    }

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBook(
        Guid id, 
        [FromBody] UpdateBookCommand command, 
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Mismatched book ID.");
        }

        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? NoContent() : HandleFailure(response);
    }

    /// <summary>
    /// Soft deletes a book.
    /// </summary>
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
    [HttpDelete("bulk")]
    public async Task<IActionResult> SoftDeleteBooksBulk(
        [FromBody] SoftDeleteBooksBulkCommand command, 
        CancellationToken cancellationToken)
    {
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? NoContent() : HandleFailure(response);
    }

    /// <summary>
    /// Retrieves the details of a book by its unique identifier.
    /// </summary>
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
