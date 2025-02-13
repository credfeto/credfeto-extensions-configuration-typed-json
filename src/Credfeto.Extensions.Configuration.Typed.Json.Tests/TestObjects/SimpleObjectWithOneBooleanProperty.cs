using System.Text.Json.Serialization;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

public sealed class SimpleObjectWithOneBooleanProperty
{
    [JsonConstructor]
    public SimpleObjectWithOneBooleanProperty(bool expected)
    {
        this.Expected = expected;
    }

    public bool Expected { get; }
}
