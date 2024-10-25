using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace LoggerDemo.Loggers
{
    public class LoggerManager
    {
        private static readonly Lazy<LoggerManager> _instance = new Lazy<LoggerManager>(() => new LoggerManager());

        public static ILogger ErrorLogger { get; set; }
        public static ILogger ApiLogger { get; set; }
        public static ILogger HttpClientLogger { get; set; }

        public static LoggerManager Instance => _instance.Value;

        public LoggerManager CreateApiLogger(ApiLogDatabaseSink apiLogDatabaseSink)
        {
            new LoggerConfiguration()
               .WriteTo.Sink(apiLogDatabaseSink)
               .CreateLogger();

            return this;
        }

        public LoggerManager CreateHttpClientLogger(HttpClientLogDatabaseSink httpClientLogDatabaseSink)
        {
            new LoggerConfiguration()
               .WriteTo.Sink(httpClientLogDatabaseSink)
               .CreateLogger();

            return this;
        }
        public LoggerManager CreateLogger(IConfiguration configuration)
        {
            ErrorLogger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .CreateLogger();

            return this;
        }
    }
}
