using System.Text.Json.Serialization;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

public sealed class SimpleObjectWithOneDecimalProperty
{
    [JsonConstructor]
    public SimpleObjectWithOneDecimalProperty(decimal expected)
    {
        this.Expected = expected;
    }

    public decimal Expected { get; }
}
