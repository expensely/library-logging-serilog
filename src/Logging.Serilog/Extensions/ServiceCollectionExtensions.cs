using Logging.Serilog.Enrichers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Serilog.Formatting.Json;

namespace Logging.Serilog.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration properties</param>
    /// <param name="maximumDepth"></param>
    /// <param name="environmentVariableName"></param>
    /// <param name="firstMessage">First message to print out</param>
    /// <param name="maximumCollection"></param>
    public static IServiceCollection AddSerilog(
        this IServiceCollection services,
        IConfiguration configuration,
        int maximumCollection = 10,
        int maximumDepth = 10,
        string environmentVariableName = "DOTNET_ENVIRONMENT",
        string firstMessage = "Logging registered")
    {
        services.AddHttpContextAccessor();
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithAssemblyName()
            .Enrich.WithAssemblyVersion()
            .Enrich.WithEnvironmentVariable(environmentVariableName, "Environment")
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
            .Enrich.With<MessageTemplate>()
            .Enrich.With<OTel>()
            .Enrich.With<RoutePattern>()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithRequestUserId()
            .Enrich.WithSpan()
            .Destructure.ToMaximumCollectionCount(maximumCollection)
            .Destructure.ToMaximumDepth(maximumDepth)
            .WriteTo.Console(new JsonFormatter(renderMessage: true))
            .CreateLogger();

        Log.Information(firstMessage);
        services.AddSingleton(Log.Logger);

        return services;
    }
}
