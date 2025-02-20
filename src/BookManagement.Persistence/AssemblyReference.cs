using System.Reflection;

namespace BookManagement.Persistence;

/// <summary> 
/// Provides a reference to the assembly containing persistence-related classes. 
/// This is used for assembly scanning and applying configurations from the assembly. 
/// </summary>
public static class AssemblyReference
{
    // Holds a reference to the current assembly
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}