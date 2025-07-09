using System.Reflection;

namespace BookManagement.Presentation;

/// <summary> 
/// Provides a reference to the current assembly. 
/// </summary>
public static class AssemblyReference
{
    /// <summary> 
    /// Holds a reference to the assembly where this class is defined. 
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
