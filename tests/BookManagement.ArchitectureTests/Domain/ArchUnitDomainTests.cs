using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using BookManagement.Domain.Events;
using BookManagement.Domain.Primitives;
using System.Reflection;

namespace BookManagement.ArchitectureTests.Domain;

public class ArchUnitDomainTests : ArchUnitBaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
     {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(DomainEvent)) // inherit from DomainEvent
            .Should()
            .BeSealed()
            .Check(Architecture);
    }

    [Fact]
    public void DomainEvents_Should_HaveDomainEventPostfix()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ImplementInterface(typeof(IDomainEvent)) 
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .Check(Architecture);
    }

    [Fact]
    public void Entities_Should_HavePrivateParameterlessConstructor()
    {
        var entityTypes = ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(Entity))
            .GetObjects(Architecture);

        var failingTypes = new List<Class>();
        foreach (var entityType in entityTypes)
        {
            var constructors = entityType.GetConstructors();

            if (!constructors.Any(c => c.Visibility == Visibility.Private
                                       && c.Parameters.Any()))
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.Should().BeEmpty();
    }
}