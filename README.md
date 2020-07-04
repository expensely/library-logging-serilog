# Expensely.Logging.Serilog

[![Build Status](https://dev.azure.com/expensely-au/Expensely/_apis/build/status/Libraries/Logging%20serilog?branchName=main)](https://dev.azure.com/expensely-au/Expensely/_build/latest?definitionId=37&branchName=main)

| View       | Badge                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
|:-----------|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Prerelease | [![Expensely.Logging.Serilog package in expensely-au@Prerelease feed in Azure Artifacts](https://feeds.dev.azure.com/expensely-au/_apis/public/Packaging/Feeds/4634f7ff-ee1a-49bd-b3de-2f19eb18d3e1@0b477f7e-e363-4441-97f7-bf3189253564/Packages/0205fb37-f302-495e-bf20-2038bcb1c5e1/Badge)](https://dev.azure.com/expensely-au/Expensely/_packaging?_a=package&feed=4634f7ff-ee1a-49bd-b3de-2f19eb18d3e1%400b477f7e-e363-4441-97f7-bf3189253564&package=0205fb37-f302-495e-bf20-2038bcb1c5e1&preferRelease=true) |
| Release    | [![Expensely.Logging.Serilog package in expensely-au@Release feed in Azure Artifacts](https://feeds.dev.azure.com/expensely-au/_apis/public/Packaging/Feeds/4634f7ff-ee1a-49bd-b3de-2f19eb18d3e1@f9bccf78-9a6f-4e24-bcd7-b5f77186974c/Packages/0205fb37-f302-495e-bf20-2038bcb1c5e1/Badge)](https://dev.azure.com/expensely-au/Expensely/_packaging?_a=package&feed=4634f7ff-ee1a-49bd-b3de-2f19eb18d3e1%40f9bccf78-9a6f-4e24-bcd7-b5f77186974c&package=0205fb37-f302-495e-bf20-2038bcb1c5e1&preferRelease=true)    |


## How to Use  
### Configuration  
| Property Name                      | Description                                                                                                                                         |
|:-----------------------------------|:----------------------------------------------------------------------------------------------------------------------------------------------------|
| SerilogConfiguration               | Serilog.Settings.Configuration config please go [here](https://github.com/serilog/serilog-settings-configuration/blob/master/README.md) for details |
| CloudWatch.MinimumLogEventLevel    | The minimum log event level required in order to write an event to the sink                                                                         |
| CloudWatch.BatchSizeLimit          | The batch size to be used when uploading logs to AWS CloudWatch                                                                                     |
| CloudWatch.QueueSizeLimit          | The queue size to be used when holding batched log events in memory                                                                                 |
| CloudWatch.Period                  | The period to be used when a batch upload should be triggered                                                                                       |
| CloudWatch.LogGroupRetentionPolicy | The number of days to retain the log events in the specified log group                                                                              |
| CloudWatch.CreateLogGroup          | Flag to indicate if the the log group should be created                                                                                             |
| CloudWatch.LogGroupName            | The log group name to be used in AWS CloudWatch                                                                                                     |
| CloudWatch.RetryAttempts           | The number of attempts to retry in the case of a failure                                                                                            |

Add Configuration
``` json
{
    "Expensely.Logging.Serilog": {
        "SerilogConfiguration": {},
        "CloudWatch": {
            "MinimumLogEventLevel" : "Verbose",
            "BatchSizeLimit" : 0,
            "QueueSizeLimit" : 0,
            "Period" : "00:00:05",
            "LogGroupRetentionPolicy" : "TwoWeeks",
            "CreateLogGroup" : false,
            "LogGroupName" : "/aws/log/group/name",
            "RetryAttempts" : 3
        }
    }
}
```


Add Expensely.Logging.Serilog
``` csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    service.AddSerilog(Configuration)
    ...
}
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
dotnet nuget push "Expensely.Logging.Serilog.*.nupkg" -Source "github"
```
