using System;
using Expensely.Logging.Serilog.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Expensely.Logging.Serilog.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services">Services</param>
        /// <param name="configuration">Configuration properties</param>
        /// <param name="configurationSectionName">Name of the configuration section </param>
        /// <param name="environmentVariableName">Name of the environment variable that contains the environment name</param>
        /// <param name="firstMessage">First message to print out</param>
        /// <returns></returns>
        public static IServiceCollection AddSerilog(
            this IServiceCollection services, 
            IConfiguration configuration,
            string configurationSectionName = "Expensely.Logging.Serilog",
            string environmentVariableName = "DOTNET_ENVIRONMENT",
            string firstMessage = "Application started")
        {
            var cloudWatchConfigurationSection = configuration.GetSection(configurationSectionName + ":CloudWatch");
            if (cloudWatchConfigurationSection != null)
                services.Configure<CloudWatchSinkOptions>(cloudWatchConfigurationSection);
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom
                .Configuration(configuration, configurationSectionName + ":SerilogConfiguration")
                .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable(environmentVariableName))
                .CreateLogger();
            Log.Information(firstMessage);
            
            return services;
        }
    }
}