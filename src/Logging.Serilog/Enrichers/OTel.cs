using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Logging.Serilog.Enrichers
{
    public class OTel : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public OTel() : this((IHttpContextAccessor) new HttpContextAccessor())
        {
        }

        public OTel(IHttpContextAccessor contextAccessor) => this._contextAccessor = contextAccessor;

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var activity = Activity.Current;

            if (activity != null)
            {
                var epochHex = activity.TraceId.ToString().Substring(0,  8);
                var randomHex = activity.TraceId.ToString().Substring(8);
                var amazonTraceId = $"1-{epochHex}-{randomHex}";
                logEvent.AddPropertyIfAbsent(new LogEventProperty("TraceId", new ScalarValue(amazonTraceId)));
                logEvent.AddPropertyIfAbsent(new LogEventProperty("SpanId", new ScalarValue(activity.SpanId)));
            }
        }
    }
}