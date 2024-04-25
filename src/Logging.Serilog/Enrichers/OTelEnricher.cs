using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Logging.Serilog.Enrichers
{
    /// <summary>
    /// Enricher for OpenTelemetry. This enricher will add a TraceId and SpanId to the log event.
    /// </summary>
    public class OTelEnricher : ILogEventEnricher
    {
        /// <summary>
        /// Enrich log event with TraceId and SpanId
        /// </summary>
        /// <param name="logEvent">Log event to enrich</param>
        /// <param name="propertyFactory">Log event property factory</param>
        public void Enrich(
            LogEvent logEvent, 
            ILogEventPropertyFactory propertyFactory)
        {
            var activity = Activity.Current;

            if (activity != null)
            {
                var epochHex = activity.TraceId.ToString().Substring(0, 8);
                var randomHex = activity.TraceId.ToString().Substring(8);
                var amazonTraceId = $"1-{epochHex}-{randomHex}";
                logEvent.AddPropertyIfAbsent(new LogEventProperty("TraceId", new ScalarValue(amazonTraceId)));
                logEvent.AddPropertyIfAbsent(new LogEventProperty("SpanId", new ScalarValue(activity.SpanId)));
            }
        }
    }
}