using Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;
using FluentValidation;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestValidators;

public sealed class SimpleObjectWithOneUInt64PropertyValidator
    : AbstractValidator<SimpleObjectWithOneUInt64Property> {}
