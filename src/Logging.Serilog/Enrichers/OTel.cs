using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Logging.Serilog.Enrichers;

public class OTel : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current;

        if (activity == null)
            return;

        var epochHex = activity.TraceId.ToString()[..8];
        var randomHex = activity.TraceId.ToString()[8..];
        var amazonTraceId = $"1-{epochHex}-{randomHex}";
        logEvent.AddPropertyIfAbsent(new LogEventProperty("TraceId", new ScalarValue(amazonTraceId)));
        logEvent.AddPropertyIfAbsent(new LogEventProperty("SpanId", new ScalarValue(activity.SpanId)));
    }
}
