using System.Text.Json.Serialization;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

public sealed class SimpleObjectWithOneProperty
{
    [JsonConstructor]
    public SimpleObjectWithOneProperty(string name)
    {
        this.Name = name;
    }

    public string Name { get; }
}