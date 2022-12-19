using System.Text.Json.Serialization;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

public sealed class SimpleObjectWithOneNullableStringProperty
{
    [JsonConstructor]
    public SimpleObjectWithOneNullableStringProperty(string? name)
    {
        this.Name = name;
    }

    public string? Name { get; }
}