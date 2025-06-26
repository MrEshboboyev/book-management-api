using System.ComponentModel;
using System.Globalization;

namespace BookManagement.Domain.Primitives.Id;

public abstract class StronglyTypedIdTypeConverter<TStrongId, TValue> : TypeConverter
{
    private readonly Func<TValue, TStrongId> _factory;

    protected StronglyTypedIdTypeConverter(Func<TValue, TStrongId> factory)
    {
        _factory = factory;
    }

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string str && !string.IsNullOrWhiteSpace(str))
        {
            TValue parsed = (TValue)Convert.ChangeType(str, typeof(TValue))!;
            return _factory(parsed);
        }

        return base.ConvertFrom(context, culture, value);
    }
}
