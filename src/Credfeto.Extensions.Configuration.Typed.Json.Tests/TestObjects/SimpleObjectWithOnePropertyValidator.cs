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