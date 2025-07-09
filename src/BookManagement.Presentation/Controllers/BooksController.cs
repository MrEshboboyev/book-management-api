using BookManagement.Application.Books.Commands.AddBook;
using BookManagement.Application.Books.Commands.AddBooksBulk;
using BookManagement.Application.Books.Commands.SoftDeleteBook;
using BookManagement.Application.Books.Commands.SoftDeleteBooksBulk;
using BookManagement.Application.Books.Commands.UpdateBook;
using BookManagement.Application.Books.Queries.Common;
using BookManagement.Application.Books.Queries.GetBookById;
using BookManagement.Application.Books.Queries.GetBooks;
using BookManagement.Domain.Enums.Users;
using BookManagement.Infrastructure.Authentication;
using BookManagement.Presentation.Abstractions;
using BookManagement.Presentation.Contracts.Books;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Presentation.Controllers;

/// <summary>
/// API Controller for managing book-related operations.
/// </summary>
// Ultra‐simplified controller
[ApiController]
[Route("api/books")]
[ServiceFilter(typeof(MediatorActionFilter))]
public sealed class BooksController : ControllerBase
{
    [HttpPost]
    [HasPermission(Permission.AddBook)]
    [MediatorEndpoint(typeof(AddBookCommand), typeof(Guid))]
    public void Add([FromBody] AddBookRequest request) { }

    [HttpPost("bulk")]
    [HasPermission(Permission.AddBooksBulk)]
    [MediatorEndpoint(typeof(AddBooksBulkCommand), typeof(List<Guid>))]
    public void AddBulk([FromBody] AddBooksBulkRequest request) { }

    [HttpPut("{id:guid}")]
    [HasPermission(Permission.UpdateBook)]
    [MediatorEndpoint(typeof(UpdateBookCommand), typeof(void))]
    public void Update(Guid id, [FromBody] UpdateBookRequest request) { }

    [HttpDelete("{id:guid}")]
    [HasPermission(Permission.SoftDeleteBook)]
    [MediatorEndpoint(typeof(SoftDeleteBookCommand), typeof(void))]
    public void SoftDelete(Guid id) { }

    [HttpDelete("bulk")]
    [HasPermission(Permission.SoftDeleteBooksBulk)]
    [MediatorEndpoint(typeof(SoftDeleteBooksBulkCommand), typeof(void))]
    public void SoftDeleteBulk([FromBody] SoftDeleteBooksBulkRequest request) { }

    [HttpGet("{id:guid}")]
    [HasPermission(Permission.GetBookDetails)]
    [MediatorEndpoint(typeof(GetBookByIdQuery), typeof(BookResponse))]
    public void GetById(Guid id) { }

    [HttpGet]
    [HasPermission(Permission.GetBooksList)]
    [MediatorEndpoint(typeof(GetBooksQuery), typeof(List<BookResponse>))]
    public void GetList(int pageNumber, int pageSize) { }
}
