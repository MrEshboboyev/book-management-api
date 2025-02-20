using System.Reflection;

namespace BookManagement.Application;

/// <summary>
/// Provides a static reference to the assembly containing application-related classes.
/// This is useful for assembly scanning, loading types, or accessing resources within the application assembly.
/// </summary>
public static class AssemblyReference
{
    // Holds a reference to the current application assembly
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}