using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Logging.Serilog.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddSerilog(
        this WebApplicationBuilder builder,
        int maximumCollection = 10,
        int maximumDepth = 10,
        string environmentVariableName = "DOTNET_ENVIRONMENT",
        string firstMessage = "Logging registered")
    {
        builder.Host.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            
            logging.Services.AddSerilog(
                builder.Configuration, 
                maximumCollection, 
                maximumDepth, 
                environmentVariableName, 
                firstMessage);

        });
        
        builder.Host.UseSerilog();
        
        return builder;
    }
}