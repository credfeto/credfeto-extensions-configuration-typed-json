using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestSerizlizationContexts;

[SuppressMessage(
    category: "ReSharper",
    checkId: "PartialTypeWithSinglePart",
    Justification = "Required for JsonSerializerContext"
)]
[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Serialization | JsonSourceGenerationMode.Metadata,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false,
    IncludeFields = false
)]
[JsonSerializable(typeof(SimpleObjectWithOneStringProperty))]
[JsonSerializable(typeof(SimpleObjectWithOneNullableStringProperty))]
[JsonSerializable(typeof(SimpleObjectWithOneBooleanProperty))]
[JsonSerializable(typeof(SimpleObjectWithOneDecimalProperty))]
[JsonSerializable(typeof(SimpleObjectWithOneInt32Property))]
[JsonSerializable(typeof(SimpleObjectWithOneUInt32Property))]
[JsonSerializable(typeof(SimpleObjectWithOneInt64Property))]
[JsonSerializable(typeof(SimpleObjectWithOneUInt64Property))]
[JsonSerializable(typeof(SimpleObjectWithArrayOfStrings))]
internal sealed partial class TestConfigurationSerializationContext : JsonSerializerContext { }
