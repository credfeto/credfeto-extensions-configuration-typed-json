using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Credfeto.Extensions.Configuration.Typed.Json.Writers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Credfeto.Extensions.Configuration.Typed.Json;

public static class TypeConfigurationExtensions
{
    public static IServiceCollection WithConfiguration<TValidator, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TSettings>(
        this IServiceCollection services,
        IConfigurationRoot configurationRoot,
        string key,
        JsonSerializerContext jsonSerializerContext)
        where TValidator : class, IValidator<TSettings>, new() where TSettings : class
    {
        return services.WithConfiguration(configurationRoot: configurationRoot, key: key, jsonSerializerContext: jsonSerializerContext, new TValidator());
    }

    public static IServiceCollection WithConfiguration<TSettings>(this IServiceCollection services,
                                                                  IConfigurationRoot configurationRoot,
                                                                  string key,
                                                                  JsonSerializerContext jsonSerializerContext,
                                                                  IValidator<TSettings> validator)
        where TSettings : class
    {
        return services.RegisterOptions(settings: DeserializeSettings(configurationRoot.GetSection(key)
                                                                                       .ToJson(jsonSerializerOptions: jsonSerializerContext.Options),
                                                                      jsonSerializerContext.GetSerializerTypeInfo<TSettings>())
                                            .Validate(validator: validator));
    }

    private static JsonTypeInfo<TSettings> GetSerializerTypeInfo<TSettings>(this JsonSerializerContext jsonSerializerContext)
        where TSettings : class
    {
        return jsonSerializerContext.GetTypeInfo(typeof(TSettings)) as JsonTypeInfo<TSettings> ?? ExceptionHelpers.RaiseNoTypeInformationAvailable<TSettings>();
    }

    [SuppressMessage(category: "FunFair.CodeAnalysis", checkId: "FFS0008: Don't disable warnings with #pragma", Justification = "Constructor has been called already and is passed to method")]
    private static IServiceCollection RegisterOptions<TSettings>(this IServiceCollection services, TSettings settings)
        where TSettings : class
    {
#pragma warning disable IL2091
        return services.AddSingleton(CreateOptions(settings));
#pragma warning restore IL2091
    }

    private static IOptions<TSettings> CreateOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TSettings>(TSettings settings)
        where TSettings : class
    {
        return Options.Create(settings);
    }

    private static TSettings DeserializeSettings<TSettings>(string result, JsonTypeInfo<TSettings> typeInfo)
        where TSettings : class
    {
        return JsonSerializer.Deserialize(json: result, jsonTypeInfo: typeInfo) ?? ExceptionHelpers.RaiseCouldNotDeserialize<TSettings>();
    }

    [SuppressMessage(category: "Meziantou.Analyzer", checkId: "MA0045:Use Async", Justification = "Not required here")]
    private static TSettings Validate<TSettings>(this TSettings settings, IValidator<TSettings> validator)
        where TSettings : class
    {
        ValidationResult validationResult = validator.Validate(settings);

        return validationResult.IsValid
            ? settings
            : ExceptionHelpers.RaiseConfigurationErrors<TSettings>(validationResult);
    }

    [SuppressMessage(category: "Meziantou.Analyzer", checkId: "MA0045:Use Async", Justification = "Not required here")]
    private static string ToJson(this IConfigurationSection section, JsonSerializerOptions jsonSerializerOptions)
    {
        ArrayBufferWriter<byte> bufferWriter = new(jsonSerializerOptions.DefaultBufferSize);

        using (Utf8JsonWriter jsonWriter = new(bufferWriter: bufferWriter, new() { Encoder = jsonSerializerOptions.Encoder, Indented = jsonSerializerOptions.WriteIndented, SkipValidation = false }))
        {
            section.SerializeObject(writer: jsonWriter, jsonSerializerOptions: jsonSerializerOptions);
        }

        return Encoding.UTF8.GetString(bufferWriter.WrittenSpan);
    }

    private static void SerializeObject(this IConfigurationSection config, Utf8JsonWriter writer, JsonSerializerOptions jsonSerializerOptions)
    {
        using (new JsonObjectWriter(writer))
        {
            IReadOnlyList<IConfigurationSection> children = config.GetChildren()
                                                                  .ToArray();

            foreach (IConfigurationSection section in children)
            {
                SerialiseSection(section: section, writer: writer, jsonSerializerOptions: jsonSerializerOptions);
            }
        }
    }

    private static void SerialiseSection(IConfigurationSection section, Utf8JsonWriter writer, JsonSerializerOptions jsonSerializerOptions)
    {
        IConfigurationSection? firstChild = section.GetChildren()
                                                   .FirstOrDefault();

        if (IsFirstArrayElement(section))
        {
            throw new ArgumentOutOfRangeException(nameof(section), actualValue: section.Path, message: "Cannot write array property name");
        }

        writer.WritePropertyName(ConvertName(jsonSerializerOptions: jsonSerializerOptions, name: section.Key));

        if (firstChild != null)
        {
            if (IsFirstArrayElement(firstChild))
            {
                SerialiseArray(config: section, writer: writer, jsonSerializerOptions: jsonSerializerOptions);
            }
            else
            {
                section.SerializeObject(writer: writer, jsonSerializerOptions: jsonSerializerOptions);
            }
        }
        else
        {
            JsonPropertyWriter.SerialiseTypedValue(configItem: section, writer: writer);
        }
    }

    private static void SerialiseArray(IConfigurationSection config, Utf8JsonWriter writer, JsonSerializerOptions jsonSerializerOptions)
    {
        using (new JsonArrayWriter(writer))
        {
            IReadOnlyList<IConfigurationSection> children = config.GetChildren()
                                                                  .ToArray();

            foreach (IConfigurationSection section in children)
            {
                if (section.GetChildren()
                           .Any())
                {
                    section.SerializeObject(writer: writer, jsonSerializerOptions: jsonSerializerOptions);
                }
                else
                {
                    JsonPropertyWriter.SerialiseTypedValue(configItem: section, writer: writer);
                }
            }
        }
    }

    private static string ConvertName(JsonSerializerOptions jsonSerializerOptions, string name)
    {
        return jsonSerializerOptions.PropertyNamingPolicy?.ConvertName(name) ?? name;
    }

    private static bool IsFirstArrayElement(IConfigurationSection section)
    {
        return section.Path.EndsWith(value: ":0", comparisonType: StringComparison.Ordinal);
    }
}