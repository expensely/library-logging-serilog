using Serilog.Core;
using Serilog.Events;

namespace Logging.Serilog.Enrichers;

public class MessageTemplate : ILogEventEnricher
{
    public void Enrich(
        LogEvent logEvent, 
        ILogEventPropertyFactory propertyFactory)
    {
        var key = "MessageTemplate";
        var value = logEvent.MessageTemplate.Text;
        var property = propertyFactory.CreateProperty(key, value);
        logEvent.AddOrUpdateProperty(property);
    }
}