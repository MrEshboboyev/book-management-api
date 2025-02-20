using System.Reflection;

namespace BookManagement.Infrastructure;

/// <summary> 
/// Provides a static reference to the assembly containing infrastructure-related classes. 
/// This is used for assembly scanning and loading types or resources from the assembly. 
/// </summary>
public static class AssemblyReference
{
    // Holds a reference to the current assembly
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}