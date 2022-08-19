using FluentValidation;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

public sealed class SimpleObjectWithOnePropertyValidator : AbstractValidator<SimpleObjectWithOneProperty>
{
    public SimpleObjectWithOnePropertyValidator()
    {
        this.RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
    }
}

public sealed class SimpleObjectWithArrayOfStringsValidator : AbstractValidator<SimpleObjectWithArrayOfStrings>
{
    public SimpleObjectWithArrayOfStringsValidator()
    {
        this.RuleFor(x => x.Items)
            .NotNull()
            .NotEmpty()
            .Must(x => x.Length > 0);

        this.RuleForEach(x => x.Items)
            .NotNull()
            .NotEmpty();
    }
}