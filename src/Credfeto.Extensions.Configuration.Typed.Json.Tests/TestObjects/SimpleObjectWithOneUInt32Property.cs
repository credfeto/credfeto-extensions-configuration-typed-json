using System.Text.Json.Serialization;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

public sealed class SimpleObjectWithOneUInt32Property
{
    [JsonConstructor]
    public SimpleObjectWithOneUInt32Property(uint expected)
    {
        this.Expected = expected;
    }

    public uint Expected { get; }
}