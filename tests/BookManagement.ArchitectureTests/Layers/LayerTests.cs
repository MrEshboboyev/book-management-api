using BookManagement.ArchitectureTests;

namespace BookManagement.ArchitectureTests.Layers;

public class LayerTests : BaseTest
{
    [Fact]
    public void Domain_Should_NotHaveDependencyOnApplications()
    {
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainLayer_Should_NotHaveDependencyOn_InfrastructureLayer()
    {
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_Should_NotHaveDependencyOn_InfrastructureLayer()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_Should_NotHaveDependencyOn_PresentationLayer()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void InfrastructureLayer_Should_NotHaveDependencyOn_PresentationLayer()
    {
        var result = Types.InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
