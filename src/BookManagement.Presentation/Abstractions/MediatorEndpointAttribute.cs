namespace BookManagement.Presentation.Abstractions;

[AttributeUsage(AttributeTargets.Method)]
public class MediatorEndpointAttribute(
    Type requestType, 
    Type responseType = null) : Attribute
{
    public Type RequestType { get; } = requestType;
    public Type ResponseType { get; } = responseType;
}
