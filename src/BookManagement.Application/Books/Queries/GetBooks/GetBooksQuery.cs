using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Books.Queries.Common;
using BookManagement.Application.Common.Models;

namespace BookManagement.Application.Books.Queries.GetBooks;

/// <summary>
/// Query to get a list of book titles with pagination.
/// </summary>
public sealed record GetBooksQuery(
    int PageNumber, 
    int PageSize) : IQuery<PaginatedList<BookTitleResponse>>;
