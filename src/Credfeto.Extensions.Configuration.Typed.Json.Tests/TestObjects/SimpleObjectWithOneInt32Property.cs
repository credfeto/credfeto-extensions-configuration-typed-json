using System.Text.Json.Serialization;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

public sealed class SimpleObjectWithOneInt32Property
{
    [JsonConstructor]
    public SimpleObjectWithOneInt32Property(int expected)
    {
        this.Expected = expected;
    }

    public int Expected { get; }
}
