using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Credfeto.Extensions.Configuration.Typed.Json.Exceptions;
using Credfeto.Extensions.Configuration.Typed.Json.Tests.TestObjects;
using Credfeto.Extensions.Configuration.Typed.Json.Tests.TestSerizlizationContexts;
using Credfeto.Extensions.Configuration.Typed.Json.Tests.TestValidators;
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
    public void SimpleObjectWithOneStringPropertyIsValid()
    {
        IOptions<SimpleObjectWithOneStringProperty> configuration = GetSetting(new Dictionary<string, string?>(StringComparer.Ordinal) { ["banana:name"] = "Qwertyuiop" },
                                                                               sectionKey: "banana",
                                                                               new SimpleObjectWithOneStringPropertyValidator());

        Assert.Equal(expected: "Qwertyuiop", actual: configuration.Value.Name);
    }

    [Fact]
    public void SimpleObjectWithOneNullableStringPropertyIsValidNull()
    {
        IOptions<SimpleObjectWithOneNullableStringProperty> configuration = GetSetting(new Dictionary<string, string?>(StringComparer.Ordinal) { ["banana:name"] = null },
                                                                                       sectionKey: "banana",
                                                                                       new SimpleObjectWithOneNullableStringPropertyValidator());

        Assert.Null(configuration.Value.Name);
    }

    [Fact]
    public void SimpleObjectWithOneNullableStringPropertyIsValidString()
    {
        IOptions<SimpleObjectWithOneNullableStringProperty> configuration = GetSetting(new Dictionary<string, string?>(StringComparer.Ordinal) { ["banana:name"] = "Qwertyuiop" },
                                                                                       sectionKey: "banana",
                                                                                       new SimpleObjectWithOneNullableStringPropertyValidator());

        Assert.Equal(expected: "Qwertyuiop", actual: configuration.Value.Name);
    }

    [Fact]
    public void SimpleObjectWithOneBooleanPropertyIsValid()
    {
        IOptions<SimpleObjectWithOneBooleanProperty> configuration = GetSetting(new Dictionary<string, string?>(StringComparer.Ordinal) { ["banana:expected"] = "true" },
                                                                                sectionKey: "banana",
                                                                                new SimpleObjectWithOneBooleanPropertyValidator());

        Assert.True(condition: configuration.Value.Expected, userMessage: "Configuration should be true");
    }

    [Fact]
    public void SimpleObjectWithOneDecimalPropertyIsValid()
    {
        IOptions<SimpleObjectWithOneDecimalProperty> configuration = GetSetting(new Dictionary<string, string?>(StringComparer.Ordinal) { ["banana:expected"] = "1.42" },
                                                                                sectionKey: "banana",
                                                                                new SimpleObjectWithOneDecimalPropertyValidator());

        Assert.Equal(expected: 1.42m, actual: configuration.Value.Expected);
    }

    [Fact]
    public void SimpleObjectWithOneInt32PropertyIsValid()
    {
        IOptions<SimpleObjectWithOneInt32Property> configuration = GetSetting(new Dictionary<string, string?>(StringComparer.Ordinal) { ["banana:expected"] = "42" },
                                                                              sectionKey: "banana",
                                                                              new SimpleObjectWithOneInt32PropertyValidator());

        Assert.Equal(expected: 42, actual: configuration.Value.Expected);
    }

    [Fact]
    public void SimpleObjectWithOneInt64PropertyIsValid()
    {
        IOptions<SimpleObjectWithOneInt64Property> configuration = GetSetting(new Dictionary<string, string?>(StringComparer.Ordinal) { ["banana:expected"] = "42" },
                                                                              sectionKey: "banana",
                                                                              new SimpleObjectWithOneInt64PropertyValidator());

        Assert.Equal(expected: 42, actual: configuration.Value.Expected);
    }

    [Fact]
    public void SimpleObjectWithOneUInt32PropertyIsValid()
    {
        IOptions<SimpleObjectWithOneUInt32Property> configuration = GetSetting(new Dictionary<string, string?>(StringComparer.Ordinal) { ["banana:expected"] = "42" },
                                                                               sectionKey: "banana",
                                                                               new SimpleObjectWithOneUInt32PropertyValidator());

        Assert.Equal(expected: 42U, actual: configuration.Value.Expected);
    }

    [Fact]
    public void SimpleObjectWithOneUInt64PropertyIsValid()
    {
        IOptions<SimpleObjectWithOneUInt64Property> configuration = GetSetting(new Dictionary<string, string?>(StringComparer.Ordinal) { ["banana:expected"] = "42" },
                                                                               sectionKey: "banana",
                                                                               new SimpleObjectWithOneUInt64PropertyValidator());

        Assert.Equal(expected: 42UL, actual: configuration.Value.Expected);
    }

    private static ValidationFailure ValidationFailure(string propertyName, string errorMessage)
    {
        return new(propertyName: propertyName, errorMessage: errorMessage);
    }

    [Fact]
    public void SimpleObjectWithOnePropertyInvalidSettings()
    {
        ConfigurationErrorsException exception = Assert.Throws<ConfigurationErrorsException>(
            () => GetSetting(new Dictionary<string, string?>(StringComparer.Ordinal) { ["banana:name"] = string.Empty }, sectionKey: "banana", new SimpleObjectWithOneStringPropertyValidator()));

        IReadOnlyList<ValidationFailure> expected =
        [
            ValidationFailure(propertyName: "Name", errorMessage: "'Name' must not be empty.")
        ];
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
            Assert.Equal(expected: expected[i].ErrorMessage, actual: actual[i].ErrorMessage);
            Assert.Equal(expected: expected[i].PropertyName, actual: actual[i].PropertyName);
        }
    }

    private static IOptions<TConfigurationType> GetSetting<TConfigurationType>(IDictionary<string, string?> settings, string sectionKey, IValidator<TConfigurationType> validator)
        where TConfigurationType : class
    {
        IServiceProvider serviceProvider = BuildServiceProvider(settings: settings, sectionKey: sectionKey, validator: validator);

        IOptions<TConfigurationType> configuration = serviceProvider.GetRequiredService<IOptions<TConfigurationType>>();
        Assert.NotNull(configuration.Value);

        return configuration;
    }

    private static IServiceProvider BuildServiceProvider<TConfigurationType>(IDictionary<string, string?> settings, string sectionKey, IValidator<TConfigurationType> validator)
        where TConfigurationType : class

    {
        IConfigurationRoot configuration = new ConfigurationBuilder().AddInMemoryCollection(settings)
                                                                     .Build();

        JsonSerializerContext jsonSerializerContext = TestConfigurationSerializationContext.Default;

        return new ServiceCollection().AddOptions()
                                      .WithConfiguration(configurationRoot: configuration, key: sectionKey, jsonSerializerContext: jsonSerializerContext, validator: validator)
                                      .BuildServiceProvider();
    }

    [Fact]
    public void SimpleObjectWithArrayOfStringsIsValid()
    {
        IOptions<SimpleObjectWithArrayOfStrings> configuration = GetSetting(new Dictionary<string, string?>(StringComparer.Ordinal) { ["banana:items:0"] = "Qwertyuiop" },
                                                                            sectionKey: "banana",
                                                                            new SimpleObjectWithArrayOfStringsValidator());

        Assert.Single(configuration.Value.Items);
        Assert.Equal(expected: "Qwertyuiop", configuration.Value.Items[0]);
    }
}