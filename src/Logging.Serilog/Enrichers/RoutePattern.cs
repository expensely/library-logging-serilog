using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Serilog.Core;
using Serilog.Events;

namespace Logging.Serilog.Enrichers
{
    /// <summary>
    /// Enricher for route pattern. This enricher will add the route pattern to the log event.
    /// </summary>
    public class RoutePattern : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Create route pattern enricher. Creates a new instance of HttpContextAccessor.
        /// </summary>
        public RoutePattern() : this(new HttpContextAccessor()) { }

        /// <summary>
        /// Create route pattern enricher.
        /// </summary>
        /// <param name="contextAccessor">HTTP context accessor</param>
        public RoutePattern(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Enrich log event with route pattern
        /// </summary>
        /// <param name="logEvent">Log event to enrich</param>
        /// <param name="propertyFactory">Log event property factory</param>
        public void Enrich(
          LogEvent logEvent,
          ILogEventPropertyFactory propertyFactory)
        {
            if (_contextAccessor.HttpContext == null)
                return;

            var routeData = _contextAccessor.HttpContext.GetRouteData();
            var routeEndpoint = _contextAccessor.HttpContext.GetEndpoint() as RouteEndpoint;
            var pattern = routeEndpoint?.RoutePattern.RawText;

            if (pattern?.Contains("{version:apiVersion}") == true)
            {
                if (routeData.Values.TryGetValue("version", out var version))
                {
                    pattern = pattern.Replace("{version:apiVersion}", version.ToString());
                }
            }

            if (pattern?.StartsWith("/") != true)
            {
                pattern = "/" + pattern;
            }

            var property = new LogEventProperty("RoutePattern", new ScalarValue(pattern));
            logEvent.AddOrUpdateProperty(property);
        }
    }
}
