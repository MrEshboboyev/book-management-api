using BookManagement.Domain.Primitives.Id;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BookManagement.Domain.Identity.Books;

[TypeConverter(typeof(BookIdTypeConverter))]
[JsonConverter(typeof(BookIdJsonConverter))]
public sealed class BookId : StronglyTypedId<GlobalId>
{
    public BookId(GlobalId value) : base(value) { }

    public static BookId New() => new(GlobalId.New());
}

public sealed class BookIdTypeConverter : StronglyTypedIdTypeConverter<BookId, int>
{
    public BookIdTypeConverter() : base(intValue => new BookId(new GlobalId(intValue))) { }
}

public sealed class BookIdJsonConverter : StronglyTypedIdJsonConverter<BookId, int>
{
    public BookIdJsonConverter() : base(intValue => new BookId(new GlobalId(intValue))) { }
}
