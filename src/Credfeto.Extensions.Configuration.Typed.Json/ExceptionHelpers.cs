using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Credfeto.Extensions.Configuration.Typed.Json.Exceptions;
using FluentValidation.Results;

namespace Credfeto.Extensions.Configuration.Typed.Json;

internal static class ExceptionHelpers
{
    [DoesNotReturn]
    public static JsonTypeInfo<TSettings> RaiseNoTypeInformationAvailable<TSettings>()
        where TSettings : class
    {
        throw new JsonException($"No Json Type Info for {typeof(TSettings).FullName}");
    }

    [DoesNotReturn]
    public static TSettings RaiseCouldNotDeserialize<TSettings>()
        where TSettings : class
    {
        throw new JsonException("Could not deserialize options");
    }

    [DoesNotReturn]
    public static TSettings RaiseConfigurationErrors<TSettings>(ValidationResult validationResult)
        where TSettings : class
    {
        throw new ConfigurationErrorsException(validationResult.Errors);
    }
}
