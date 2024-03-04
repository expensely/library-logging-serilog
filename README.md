# Logging.Serilog

![Pipeline](https://github.com/gavanlamb/library-logging-serilog/actions/workflows/build-and-release.yml/badge.svg?event=pull_request&branch=main)
![NuGet version](https://gavanlamb-github-actions-assets.s3.ap-southeast-2.amazonaws.com/gavanlamb/library-logging-serilog/main/version/version.svg)

[Code coverage report](https://gavanlamb-github-actions-assets.s3.ap-southeast-2.amazonaws.com/gavanlamb/library-logging-serilog/main/coverage/Html/index.html)
![Combined coverage](https://gavanlamb-github-actions-assets.s3.ap-southeast-2.amazonaws.com/gavanlamb/library-logging-serilog/main/coverage/Badges/badge_combined.svg)
![Line coverage](https://gavanlamb-github-actions-assets.s3.ap-southeast-2.amazonaws.com/gavanlamb/library-logging-serilog/main/coverage/Badges/badge_linecoverage.svg)
![Branch coverage](https://gavanlamb-github-actions-assets.s3.ap-southeast-2.amazonaws.com/gavanlamb/library-logging-serilog/main/coverage/Badges/badge_branchcoverage.svg)
![Method coverage](https://gavanlamb-github-actions-assets.s3.ap-southeast-2.amazonaws.com/gavanlamb/library-logging-serilog/main/coverage/Badges/badge_methodcoverage.svg)

## How to Use

### Configuration

| Property Name | Description                                                                                                                  |
|:--------------|:-----------------------------------------------------------------------------------------------------------------------------|
| Serilog       | Serilog config please go [here](https://github.com/serilog/serilog-settings-configuration/blob/master/README.md) for details |

Add Configuration

``` json
{
  "Serilog": {
    "MinimumLevel": "Warning",
    "Override": {
      "Microsoft.AspNetCore": "Warning",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Warning",
      "Microsoft.EntityFrameworkCore": "Debug"
    }
  }
}
```

Add Serilog to `IHostBuilder`

``` csharp
public void ConfigureServices(IServiceCollection services)
{
    IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services => { services.AddHostedService<Worker>(); })
        .AddSerilog()
        .Build();
    ...
}
```

Add Serilog to `WebApplicationBuilder`

``` csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddSerilog();
```

## Development

### Build, Package & Release

#### Locally

```
// Step 1: Authenticate
dotnet build --configuration release 

// Step 2: Pack
dotnet pack --configuration release 

// Step 3: Publish
dotnet nuget push "Logging.Serilog.*.nupkg" -Source "github"
```

## Approaches
### Build

### Tests
#### Code coverage

#### Mutation testing

### Visibility and discoverability