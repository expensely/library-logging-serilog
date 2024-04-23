using System;
using Logging.Serilog.Enrichers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Serilog.Formatting.Json;

namespace Logging.Serilog.Extensions;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Add Serilog to webApplicationBuilder
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <param name="maximumCollection"></param>
    /// <param name="maximumDepth"></param>
    /// <param name="environmentVariableName"></param>
    /// <param name="firstMessage"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <list type="bullet">
    ///         <item>When the <paramref name="webApplicationBuilder"/> parameter is null.</item>
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
    public static WebApplicationBuilder AddSerilog(
        this WebApplicationBuilder webApplicationBuilder, // https://andrewlock.net/exploring-dotnet-6-part-2-comparing-webapplicationbuilder-to-the-generic-host/
        int maximumCollection = 10,
        int maximumDepth = 10,
        string environmentVariableName = "DOTNET_ENVIRONMENT",
        string firstMessage = "Logging registered")
    {
        if (webApplicationBuilder == null)
            throw new ArgumentNullException($"{nameof(webApplicationBuilder)} cannot be null", nameof(webApplicationBuilder));

        if (environmentVariableName == null)
            throw new ArgumentNullException($"{nameof(environmentVariableName)} cannot be null", nameof(environmentVariableName));
        if (string.IsNullOrWhiteSpace(environmentVariableName))
            throw new ArgumentException($"{nameof(environmentVariableName)} cannot be empty", nameof(environmentVariableName));

        if (firstMessage == null)
            throw new ArgumentNullException($"{nameof(firstMessage)} cannot be null", nameof(firstMessage));
        if (string.IsNullOrWhiteSpace(firstMessage))
            throw new ArgumentException($"{nameof(firstMessage)} cannot be empty", nameof(firstMessage));

        webApplicationBuilder.Logging.ClearProviders();
        webApplicationBuilder.Services.AddHttpContextAccessor();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(webApplicationBuilder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithAssemblyName()
            .Enrich.WithAssemblyVersion()
            .Enrich.WithEnvironmentVariable(environmentVariableName, "Environment")
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
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
            .WriteTo.Console(new JsonFormatter(renderMessage: false))
            .CreateLogger();
        Log.Information(firstMessage);
        webApplicationBuilder.Logging.AddSerilog(Log.Logger);

        webApplicationBuilder.Host.UseSerilog();

        return webApplicationBuilder;
    }
}
