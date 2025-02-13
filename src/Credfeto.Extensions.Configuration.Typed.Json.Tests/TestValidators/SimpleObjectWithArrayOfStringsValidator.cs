using Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;
using FluentValidation;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestValidators;

public sealed class SimpleObjectWithArrayOfStringsValidator
    : AbstractValidator<SimpleObjectWithArrayOfStrings>
{
    public SimpleObjectWithArrayOfStringsValidator()
    {
        this.RuleFor(x => x.Items).NotNull().NotEmpty().Must(x => x.Length > 0);

        this.RuleForEach(x => x.Items).NotNull().NotEmpty();
    }
}
