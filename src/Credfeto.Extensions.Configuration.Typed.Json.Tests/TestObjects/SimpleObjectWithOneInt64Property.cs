using System.Text.Json.Serialization;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

public sealed class SimpleObjectWithOneInt64Property
{
    [JsonConstructor]
    public SimpleObjectWithOneInt64Property(long expected)
    {
        this.Expected = expected;
    }

    public long Expected { get; }
}