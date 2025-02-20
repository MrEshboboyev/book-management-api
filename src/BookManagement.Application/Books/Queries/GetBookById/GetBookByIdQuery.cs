using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Books.Queries.Common;

namespace BookManagement.Application.Books.Queries.GetBookById;

public sealed record GetBookByIdQuery(Guid Id) : IQuery<BookResponse>;
