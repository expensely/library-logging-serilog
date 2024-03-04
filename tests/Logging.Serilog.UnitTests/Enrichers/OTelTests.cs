using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoFixture;
using AutoFixture.AutoMoq;
using Logging.Serilog.Enrichers;
using Serilog.Core;
using Serilog.Events;
using Xunit;

namespace Logging.Serilog.UnitTests.Enrichers;

public class OTelTests
{
    private readonly IFixture _fixture = new Fixture().Customize(
        new AutoMoqCustomization { ConfigureMembers = true }
    );

    [Fact]
    public void Enrich_ActivityIsNull_PropertiesNotAdded()
    {
        // Arrange
        Activity.Current = null;

        var logEvent = _fixture.Build<LogEvent>()
            .FromFactory<int>(_ => new LogEvent(
                _fixture.Create<DateTimeOffset>(),
                _fixture.Create<LogEventLevel>(),
                null,
                _fixture.Create<MessageTemplate>(),
                new List<LogEventProperty>()))
            .Create();

        var propertyFactory = _fixture.Create<ILogEventPropertyFactory>();

        var enricher = _fixture.Create<OTel>();

        // Act
        enricher.Enrich(logEvent, propertyFactory);

        // Assert
        Assert.Empty(logEvent.Properties);
    }

    [Fact]
    public void Enrich_ActivityIsNotNull_PropertiesNotAdded()
    {
        // Arrange
        Activity.Current = _fixture.Build<Activity>()
            .FromFactory<int>(_ => new Activity("Unit test")
                .SetParentId(new ActivityTraceId(), new ActivitySpanId(), ActivityTraceFlags.Recorded)
                .Start())
            .Create();

        var logEvent = _fixture.Build<LogEvent>()
            .FromFactory<int>(_ => new LogEvent(
                _fixture.Create<DateTimeOffset>(),
                _fixture.Create<LogEventLevel>(),
                null,
                _fixture.Create<MessageTemplate>(),
                new List<LogEventProperty>()))
            .Create();

        var propertyFactory = _fixture.Create<ILogEventPropertyFactory>();

        var enricher = _fixture.Create<OTel>();

        // Act
        enricher.Enrich(logEvent, propertyFactory);

        // Assert
        Assert.Contains(logEvent.Properties, p => p.Key == "TraceId");
        Assert.Contains(logEvent.Properties, p => p.Key == "SpanId");
    }
}
