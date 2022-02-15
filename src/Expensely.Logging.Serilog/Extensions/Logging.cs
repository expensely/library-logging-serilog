using Expensely.Logging.Serilog.Enrichers;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Serilog.Formatting.Json;

namespace Expensely.Logging.Serilog.Extensions
{
    public static class Logging
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration">Configuration properties</param>
        /// <param name="environmentVariableName">Name of the environment variable that contains the environment name</param>
        /// <param name="firstMessage">First message to print out</param>
        public static void AddSerilog(
            IConfiguration configuration,
            string environmentVariableName = "DOTNET_ENVIRONMENT",
            string firstMessage = "Logging registered")
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithAssemblyName()
                .Enrich.WithAssemblyVersion()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", configuration.GetValue<string>(environmentVariableName))
                .Enrich.FromLogContext()
                .Enrich.WithMessageTemplate()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.WithExceptionDetails()
                .Enrich.WithRequestUserId()
                .Enrich.With<RoutePattern>()
                .Enrich.With<OTel>()
                .Enrich.WithSpan()
                .WriteTo.Console(new JsonFormatter(renderMessage: true))
                .CreateLogger();

            Log.Information(firstMessage);
        }
    }
}