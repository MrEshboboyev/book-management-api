using BookManagement.Application.Books.Queries.Common.Responses;
using BookManagement.Application.Common.Messaging;
using BookManagement.Application.Common.Pagination;

namespace BookManagement.Application.Books.Queries.GetBooks;

/// <summary>
/// Query to get a list of book titles with pagination.
/// </summary>
public sealed record GetBooksQuery(
    int PageNumber, 
    int PageSize) : IQuery<PaginatedList<BookTitleResponse>>;
