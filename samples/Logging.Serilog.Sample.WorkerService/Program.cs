using Logging.Serilog.Extensions;
using Logging.Serilog.Sample.WorkerService;
using Serilog;

try
{
    IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services => { services.AddHostedService<Worker>(); })
        .AddSerilog()
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}