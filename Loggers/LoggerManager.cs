using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;

namespace LoggerDemo.Loggers
{
    public class LoggerManager
    {
        private static readonly Lazy<LoggerManager> _instance = new Lazy<LoggerManager>(() => new LoggerManager());

        public static ILogger AppLogger { get; set; }
        public static ILogger ApiLogger { get; set; }
        public static ILogger HttpClientLogger { get; set; }

        public static LoggerManager Instance => _instance.Value;

        public LoggerManager CreateAppLogger(IConfiguration configuration)
        {
            var columnOptions = new ColumnOptions();
            columnOptions.Store.Remove(StandardColumn.Properties);

            AppLogger = new LoggerConfiguration()
                .MinimumLevel.Information()
                //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                //.MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Information)
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: configuration.GetConnectionString("DefaultConnection"),  // Update with your connection string
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "AppLogs",  // Set custom table name
                        AutoCreateSqlTable = true  // Automatically create table
                    },
                    columnOptions: columnOptions)
               .CreateLogger();

            return this;
        }

        public LoggerManager CreateApiLogger(ApiLogDatabaseSink apiLogDatabaseSink)
        {
            ApiLogger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Sink(apiLogDatabaseSink)
                .CreateLogger();

            return this;
        }

        public LoggerManager CreateHttpClientLogger(HttpClientLogDatabaseSink httpClientLogDatabaseSink)
        {
            HttpClientLogger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Sink(httpClientLogDatabaseSink)
                .CreateLogger();

            return this;
        }
    }
}
