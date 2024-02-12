using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;

namespace Logging.Serilog.Extensions;

public static class IHostBuilderExtensions
{
    /// <summary>
    /// Add Serilog to hostBuilder
    /// </summary>
    /// <param name="hostBuilder"></param>
    /// <param name="maximumCollection"></param>
    /// <param name="maximumDepth"></param>
    /// <param name="environmentVariableName"></param>
    /// <param name="firstMessage"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <list type="bullet">
    ///         <item>When the <paramref name="hostBuilder"/> parameter is null.</item>
    ///         <item>When the <paramref name="environmentVariableName"/> parameter is null.</item>
    ///         <item>When the <paramref name="firstMessage"/> parameter is null.</item>
    ///     </list>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <list type="bullet">
    ///         <item>When the <paramref name="environmentVariableName"/> parameter is empty or whitespace.</item>
    ///         <item>When the <paramref name="firstMessage"/> parameter is empty or whitespace.</item>
    ///     </list>
    /// </exception>
    public static IHostBuilder AddSerilog(
        this IHostBuilder hostBuilder,
        int maximumCollection = 10,
        int maximumDepth = 10,
        string environmentVariableName = "DOTNET_ENVIRONMENT",
        string firstMessage = "Logging registered")
    {
        if (hostBuilder == null)
            throw new ArgumentNullException($"{nameof(hostBuilder)} cannot be null", nameof(hostBuilder));

        if (environmentVariableName == null)
            throw new ArgumentNullException($"{nameof(environmentVariableName)} cannot be null", nameof(environmentVariableName));
        if (string.IsNullOrWhiteSpace(environmentVariableName))
            throw new ArgumentException($"{nameof(environmentVariableName)} cannot be empty", nameof(environmentVariableName));

        if (firstMessage == null)
            throw new ArgumentNullException($"{nameof(firstMessage)} cannot be null", nameof(firstMessage));
        if (string.IsNullOrWhiteSpace(firstMessage))
            throw new ArgumentException($"{nameof(firstMessage)} cannot be empty", nameof(firstMessage));

        hostBuilder
            .ConfigureLogging((hostContext, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
            
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(hostContext.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithAssemblyName()
                    .Enrich.WithAssemblyVersion()
                    .Enrich.WithEnvironmentVariable(environmentVariableName, "Environment")
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .Enrich.WithThreadName()
                    .Enrich.WithProcessId()
                    .Enrich.WithProcessName()
                    .Destructure.ToMaximumCollectionCount(maximumCollection)
                    .Destructure.ToMaximumDepth(maximumDepth)
                    .WriteTo.Console(new JsonFormatter(renderMessage: false))
                    .CreateLogger();
                Log.Information(firstMessage);
                
                loggingBuilder.Services.AddSingleton(Log.Logger);
            })
            .UseSerilog();

        return hostBuilder;
    }
}