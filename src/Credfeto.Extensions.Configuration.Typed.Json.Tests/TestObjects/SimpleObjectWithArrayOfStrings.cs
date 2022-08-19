using System.Text.Json.Serialization;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

public sealed class SimpleObjectWithArrayOfStrings
{
    [JsonConstructor]
    public SimpleObjectWithArrayOfStrings(string[] items)
    {
        this.Items = items;
    }

    public string[] Items { get; }
}