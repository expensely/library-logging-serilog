using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Logging.Serilog.Enrichers;
using Serilog.Core;
using Serilog.Events;
using Xunit;

namespace Logging.Serilog.UnitTests.Enrichers;

public class OTelEnricherTests
{
    private readonly IFixture _fixture =
        new Fixture().Customize(new AutoMoqCustomization() { ConfigureMembers = true });

    [Fact]
    public void Enrich_NullLogEvent_ThrowsArgumentNullException()
    {
        // Arrange
        var enricher = new OTelEnricher();
        LogEvent? logEvent = null;
        ILogEventPropertyFactory propertyFactory = _fixture.Create<ILogEventPropertyFactory>();

        // Act
        Action act = () => enricher.Enrich(logEvent!, propertyFactory);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("logEvent (Parameter 'logEvent cannot be null')");
    }

    [Fact]
    public void Enrich_NullPropertyFactory_ThrowsArgumentNullException()
    {
        // Arrange
        var enricher = new OTelEnricher();
        LogEvent logEvent = _fixture.Create<LogEvent>();
        ILogEventPropertyFactory? propertyFactory = null;

        // Act
        Action act = () => enricher.Enrich(logEvent, propertyFactory!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("propertyFactory (Parameter 'propertyFactory cannot be null')");
    }
}