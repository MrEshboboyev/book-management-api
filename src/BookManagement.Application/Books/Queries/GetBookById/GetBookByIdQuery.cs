using BookManagement.Application.Books.Queries.Common.Responses;
using BookManagement.Application.Common.Messaging;

namespace BookManagement.Application.Books.Queries.GetBookById;

public sealed record GetBookByIdQuery(Guid Id) : IQuery<BookResponse>;
