using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

public sealed class SimpleObjectWithArrayOfStrings
{
    [SuppressMessage(category: "Meziantou.Analyzers", checkId: "MA0109: Add an overload with Span<T>", Justification = "Would not work here")]
    [JsonConstructor]
    public SimpleObjectWithArrayOfStrings(string[] items)
    {
        this.Items = items;
    }

    public string[] Items { get; }
}