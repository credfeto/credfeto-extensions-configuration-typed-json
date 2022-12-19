using System;
using System.Text.Json;

namespace Credfeto.Extensions.Configuration.Typed.Json.Writers;

internal sealed class JsonObjectWriter : IDisposable
{
    private readonly Utf8JsonWriter _writer;

    public JsonObjectWriter(Utf8JsonWriter writer)
    {
        this._writer = writer;
        this._writer.WriteStartObject();
    }

    public void Dispose()
    {
        this._writer.WriteEndObject();
    }
}