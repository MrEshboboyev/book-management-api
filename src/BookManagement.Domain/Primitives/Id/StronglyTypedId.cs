namespace BookManagement.Domain.Primitives.Id;

public abstract class StronglyTypedId<T> where T : IGlobalIdentityValue
{
    public T Value { get; }

    protected StronglyTypedId(T value)
    {
        Value = value;
    }

    public override string ToString() => Value.AsString();

    public override bool Equals(object obj)
        => obj is StronglyTypedId<T> other && EqualityComparer<T>.Default.Equals(Value, other.Value);

    public override int GetHashCode() => Value.GetHashCode();
}
