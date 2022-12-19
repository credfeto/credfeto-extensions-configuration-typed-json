using System;
using System.Text.Json;

namespace Credfeto.Extensions.Configuration.Typed.Json.Writers;

internal sealed class JsonArrayWriter : IDisposable
{
    private readonly Utf8JsonWriter _writer;

    public JsonArrayWriter(Utf8JsonWriter writer)
    {
        this._writer = writer;
        this._writer.WriteStartArray();
    }

    public void Dispose()
    {
        this._writer.WriteEndArray();
    }
}