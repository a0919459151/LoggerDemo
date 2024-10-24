using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace LoggerDemo.Loggers
{
    public class LoggerManager
    {
        private readonly IServiceProvider _serviceProvider;

        private ILogger _apiLogger;
        private ILogger _httpClientLogger;

        public LoggerManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // Get ApiLogger
        public ILogger GetApiLogger()
        {
            if (_apiLogger is null)
            {
                // init logger in LoggerManager scoped lifetime
                _apiLogger = new LoggerConfiguration()
                   .WriteTo.Sink(_serviceProvider.GetRequiredService<ApiLogDatabaseSink>())
                   .CreateLogger();
            }
            
            return _apiLogger;
        }

        // Get HttpClientLogger
        public ILogger GetHttpClientLogger()
        {
            if (_httpClientLogger is null)
            {
                // init logger in LoggerManager scoped lifetime
                _httpClientLogger = new LoggerConfiguration()
                    .WriteTo.Sink(_serviceProvider.GetRequiredService<HttpClientLogDatabaseSink>())
                    .CreateLogger();
            }

            return _httpClientLogger;
        }
    }
}
