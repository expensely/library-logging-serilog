using System;
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
        /// <exception cref="ArgumentNullException">
        ///     <list type="bullet">
        ///         <item>When the <paramref name="logEvent"/> parameter is null.</item>
        ///         <item>When the <paramref name="propertyFactory"/> parameter is null.</item>
        ///     </list>
        /// </exception>
        public void Enrich(
            LogEvent logEvent,
            ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null)
                throw new ArgumentNullException($"{nameof(logEvent)} cannot be null", nameof(logEvent));
            if (propertyFactory == null)
                throw new ArgumentNullException($"{nameof(propertyFactory)} cannot be null", nameof(propertyFactory));

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