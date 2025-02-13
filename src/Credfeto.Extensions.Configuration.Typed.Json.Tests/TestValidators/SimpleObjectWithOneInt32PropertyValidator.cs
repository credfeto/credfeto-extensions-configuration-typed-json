using Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;
using FluentValidation;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestValidators;

public sealed class SimpleObjectWithOneInt32PropertyValidator
    : AbstractValidator<SimpleObjectWithOneInt32Property> { }
