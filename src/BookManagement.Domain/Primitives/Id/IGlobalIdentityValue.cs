namespace BookManagement.Domain.Primitives.Id;

public interface IGlobalIdentityValue
{
    object Value { get; }
    string AsString();
}
