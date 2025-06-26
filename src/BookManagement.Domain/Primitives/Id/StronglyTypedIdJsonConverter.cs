using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookManagement.Domain.Primitives.Id;

public abstract class StronglyTypedIdJsonConverter<TStrongId, TValue> : JsonConverter<TStrongId>
{
    private readonly Func<TValue, TStrongId> _factory;

    protected StronglyTypedIdJsonConverter(Func<TValue, TStrongId> factory)
    {
        _factory = factory;
    }

    public override TStrongId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string raw = reader.GetString();
        TValue parsed = (TValue)Convert.ChangeType(raw, typeof(TValue))!;
        return _factory(parsed);
    }

    public override void Write(Utf8JsonWriter writer, TStrongId value, JsonSerializerOptions options)
    {
        var valueProperty = value!.GetType().GetProperty("Value")!;
        var innerValue = valueProperty.PropertyType.GetProperty("InnerValue")!.GetValue(valueProperty.GetValue(value))!;
        writer.WriteStringValue(innerValue.ToString());
    }
}
