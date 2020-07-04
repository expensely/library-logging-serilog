using System;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using Serilog.Sinks.AwsCloudWatch;

namespace Expensely.Logging.Serilog.Settings
{
    public class CloudWatchSinkOptions : ICloudWatchSinkOptions
    {
        public LogEventLevel MinimumLogEventLevel { get; }
        public int BatchSizeLimit { get; }
        public int QueueSizeLimit { get; }
        public TimeSpan Period { get; }
        public LogGroupRetentionPolicy LogGroupRetentionPolicy { get; set; }
        public bool CreateLogGroup { get; }
        public string LogGroupName { get; }
        public ILogStreamNameProvider LogStreamNameProvider { get; } = new DefaultLogStreamProvider();
        public ITextFormatter TextFormatter { get; } = new JsonFormatter();
        public byte RetryAttempts { get; }
    }
}
