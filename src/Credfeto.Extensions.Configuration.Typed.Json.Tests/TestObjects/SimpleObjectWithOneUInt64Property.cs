using System.Text.Json.Serialization;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

public sealed class SimpleObjectWithOneUInt64Property
{
    [JsonConstructor]
    public SimpleObjectWithOneUInt64Property(ulong expected)
    {
        this.Expected = expected;
    }

    public ulong Expected { get; }
}
