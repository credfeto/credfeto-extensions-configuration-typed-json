using Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;
using FluentValidation;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestValidators;

public sealed class SimpleObjectWithOneStringPropertyValidator : AbstractValidator<SimpleObjectWithOneStringProperty>
{
    public SimpleObjectWithOneStringPropertyValidator()
    {
        this.RuleFor(x => x.Name).NotNull().NotEmpty();
    }
}
