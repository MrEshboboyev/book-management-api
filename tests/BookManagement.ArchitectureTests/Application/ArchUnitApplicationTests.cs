using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using FluentValidation;
using BookManagement.Application.Common.Messaging;

namespace BookManagement.ArchitectureTests.Application;

public class ArchUnitApplicationTests : ArchUnitBaseTest
{
    [Fact]
    public void CommandHandler_ShouldHave_NameEndingWith_CommandHandler()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .Check(Architecture);
    }

    [Fact]
    public void CommandHandler_Should_NotBePublic()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .NotBePublic()
            .Check(Architecture);
    }

    [Fact]
    public void QueryHandler_ShouldHave_NameEndingWith_QueryHandler()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .Check(Architecture);
    }

    [Fact]
    public void QueryHandler_Should_NotBePublic()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .NotBePublic()
            .Check(Architecture);
    }

    [Fact]
    public void Validator_ShouldHave_NameEndingWith_Validator()
    {
       ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(AbstractValidator<>))
            .Should()
            .HaveNameEndingWith("Validator")
            .Check(Architecture);
    }

    [Fact]
    public void Validator_Should_NotBePublic()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(AbstractValidator<>))
            .Should()
            .NotBePublic()
            .Check(Architecture);
    }
}
