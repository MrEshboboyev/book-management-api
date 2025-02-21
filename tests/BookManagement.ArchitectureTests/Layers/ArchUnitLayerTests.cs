using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;

namespace BookManagement.ArchitectureTests.Layers;

public class ArchUnitLayerTests : ArchUnitBaseTest
{
    [Fact]
    public void Domain_Should_NotHaveDependencyOnApplications()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(DomainLayer)
            .Should()
            .NotDependOnAny(ApplicationLayer)
            .Check(Architecture);
    }

    [Fact]
    public void DomainLayer_Should_NotHaveDependencyOn_InfrastructureLayer()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(DomainLayer)
            .Should()
            .NotDependOnAny(InfrastructureLayer)
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_Should_NotHaveDependencyOn_InfrastructureLayer()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(ApplicationLayer)
            .Should()
            .NotDependOnAny(InfrastructureLayer)
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_Should_NotHaveDependencyOn_PresentationLayer()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(ApplicationLayer)
            .Should()
            .NotDependOnAny(PresentationLayer)
            .Check(Architecture);
    }

    [Fact]
    public void InfrastructureLayer_Should_NotHaveDependencyOn_PresentationLayer()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(InfrastructureLayer)
            .Should()
            .NotDependOnAny(PresentationLayer)
            .Check(Architecture);
    }
}
