using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;
using FluentValidation;
using FluentValidation.Results;
using FunFair.Test.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace Credfeto.Extensions.Configuration.Typed.Json.Tests;

public sealed class TypeConfigurationExtensionsTests : LoggingTestBase
{
    public TypeConfigurationExtensionsTests(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public void SimpleObjectWithOnePropertyIsValid()
    {
        IOptions<SimpleObjectWithOneProperty> configuration = GetSetting(new Dictionary<string, string> { ["banana:name"] = "Qwertyuiop" },
                                                                         sectionKey: "banana",
                                                                         new SimpleObjectWithOnePropertyValidator());

        Assert.Equal(expected: "Qwertyuiop", actual: configuration.Value.Name);
    }

    [Fact]
    public void SimpleObjectWithOnePropertyInvalidSettings()
    {
        ConfigurationErrorsException exception =
            Assert.Throws<ConfigurationErrorsException>(() => GetSetting(new Dictionary<string, string> { ["banana:name"] = string.Empty },
                                                                         sectionKey: "banana",
                                                                         new SimpleObjectWithOnePropertyValidator()));

        IReadOnlyList<ValidationFailure> expected = new ValidationFailure[]
                                                    {
                                                        new(propertyName: "Name", errorMessage: "'Name' must not be empty.")
                                                    };
        this.Dump(exception.Errors);
        this.AssertIdentical(expected: expected, actual: exception.Errors);
    }

    private void AssertIdentical(IReadOnlyList<ValidationFailure> expected, IReadOnlyList<ValidationFailure> actual)
    {
        Assert.Equal(expected: expected.Count, actual: actual.Count);

        for (int i = 0; i < expected.Count; i++)
        {
            this.Output.WriteLine($"* Item {i}:");
            this.Output.WriteLine($"-> Name: {expected[i].PropertyName} <> {actual[i].PropertyName}");
            this.Output.WriteLine($"-> Message: {expected[i].ErrorMessage} <> {actual[i].ErrorMessage}");
            Assert.Equal(expected: expected[i]
                             .ErrorMessage,
                         actual: actual[i]
                             .ErrorMessage);
            Assert.Equal(expected: expected[i]
                             .PropertyName,
                         actual: actual[i]
                             .PropertyName);
        }
    }

    private void Dump(IReadOnlyList<ValidationFailure> errors)
    {
        foreach (ValidationFailure validationError in errors)
        {
            this.Output.WriteLine($"{validationError.PropertyName}: {validationError.ErrorMessage}");
        }
    }

    private static IOptions<TConfigurationType> GetSetting<TConfigurationType>(IDictionary<string, string> settings, string sectionKey, IValidator<TConfigurationType> validator)
        where TConfigurationType : class
    {
        IServiceProvider serviceProvider = BuildServiceProvider(settings: settings, sectionKey: sectionKey, validator: validator);

        IOptions<TConfigurationType> configuration = serviceProvider.GetRequiredService<IOptions<TConfigurationType>>();
        Assert.NotNull(configuration.Value);

        return configuration;
    }

    private static IServiceProvider BuildServiceProvider<TConfigurationType>(IDictionary<string, string> settings, string sectionKey, IValidator<TConfigurationType> validator)
        where TConfigurationType : class

    {
        IConfigurationRoot configuration = new ConfigurationBuilder().AddInMemoryCollection(settings)
                                                                     .Build();

        JsonSerializerContext jsonSerializerContext = TestConfigurationSerializationContext.Default;

        return new ServiceCollection().AddOptions()
                                      .WithConfiguration(configurationRoot: configuration, key: sectionKey, jsonSerializerContext: jsonSerializerContext, validator: validator)
                                      .BuildServiceProvider();
    }
}