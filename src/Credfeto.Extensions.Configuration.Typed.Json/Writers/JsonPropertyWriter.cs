using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Credfeto.Extensions.Configuration.Typed.Json.Writers;

internal static class JsonPropertyWriter
{
    private static bool WriteBooleanValue(IConfigurationSection configItem, Utf8JsonWriter writer)
    {
        if (bool.TryParse(value: configItem.Value, out bool boolean))
        {
            writer.WriteBooleanValue(value: boolean);

            return true;
        }

        return false;
    }

    private static bool WriteIntegerValue(IConfigurationSection configItem, Utf8JsonWriter writer)
    {
        if (long.TryParse(s: configItem.Value, style: NumberStyles.Integer, provider: CultureInfo.InvariantCulture, out long integer))
        {
            writer.WriteNumberValue(value: integer);

            return true;
        }

        return false;
    }

    private static bool WriteDecimalValue(IConfigurationSection configItem, Utf8JsonWriter writer)
    {
        if (decimal.TryParse(s: configItem.Value, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, out decimal real))
        {
            writer.WriteNumberValue(value: real);

            return true;
        }

        return false;
    }

    private static bool WriteNullValue(IConfigurationSection configItem, Utf8JsonWriter writer)
    {
        if (configItem.Value == null)
        {
            writer.WriteNullValue();

            return true;
        }

        return false;
    }

    public static void SerialiseTypedValue(IConfigurationSection configItem, Utf8JsonWriter writer)
    {
        if (WriteTypedProperties(configItem: configItem, writer: writer))
        {
            return;
        }

        writer.WriteStringValue(value: configItem.Value);
    }

    private static bool WriteTypedProperties(IConfigurationSection configItem, Utf8JsonWriter writer)
    {
        return WriteNullValue(configItem: configItem, writer: writer) || WriteBooleanValue(configItem: configItem, writer: writer) || WriteDecimalValue(configItem: configItem, writer: writer) ||
               WriteIntegerValue(configItem: configItem, writer: writer);
    }
}