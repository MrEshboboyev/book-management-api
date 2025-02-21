using Assembly = System.Reflection.Assembly;

namespace BookManagement.ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(BookManagement.Domain.AssemblyReference).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(BookManagement.Application.AssemblyReference).Assembly;
    protected static readonly Assembly PersistenceAssembly = typeof(Persistence.AssemblyReference).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.AssemblyReference).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(Presentation.AssemblyReference).Assembly;
}
