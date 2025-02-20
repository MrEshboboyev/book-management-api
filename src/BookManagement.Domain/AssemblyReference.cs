using System.Reflection;

namespace BookManagement.Domain;

/// <summary>
/// Provides a static reference to the assembly containing domain-related classes.
/// This is useful for assembly scanning, loading types, or accessing resources within the domain assembly.
/// </summary>
public static class AssemblyReference
{
    // Holds a reference to the current domain assembly
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}