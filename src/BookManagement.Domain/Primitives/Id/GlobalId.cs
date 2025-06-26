namespace BookManagement.Domain.Primitives.Id;

public readonly record struct GlobalId(int InnerValue) : IGlobalIdentityValue
{
    // EF Core avtomatik belgilaydi, shu sababli 0 yoki default beramiz
    public static GlobalId New() => new(0);

    public object Value => InnerValue;
    public string AsString() => InnerValue.ToString();
}
