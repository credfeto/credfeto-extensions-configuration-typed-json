using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;

[SuppressMessage(category: "ReSharper", checkId: "PartialTypeWithSinglePart", Justification = "Required for JsonSerializerContext")]
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization | JsonSourceGenerationMode.Metadata,
                             PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
                             DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                             WriteIndented = false,
                             IncludeFields = false)]
[JsonSerializable(typeof(SimpleObjectWithOneProperty))]
internal sealed partial class TestConfigurationSerializationContext : JsonSerializerContext
{
}