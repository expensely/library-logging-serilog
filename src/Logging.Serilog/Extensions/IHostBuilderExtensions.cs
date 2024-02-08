using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Logging.Serilog.Extensions;

public static class IHostBuilderExtensions
{
    public static IHostBuilder AddSerilog(
        this IHostBuilder builder,
        int maximumCollection = 10,
        int maximumDepth = 10,
        string environmentVariableName = "DOTNET_ENVIRONMENT",
        string firstMessage = "Logging registered")
    {
        builder.ConfigureLogging((hostContext, loggingBuilder) =>
        {
            loggingBuilder.ClearProviders();
            
            loggingBuilder.Services.AddSerilog(
                hostContext.Configuration, 
                maximumCollection, 
                maximumDepth, 
                environmentVariableName, 
                firstMessage);
        });
        
        builder.UseSerilog();
        
        return builder;
    }
}