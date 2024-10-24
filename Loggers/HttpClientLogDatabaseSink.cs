using LoggerDemo.DbEntities;
using LoggerDemo.Repositories;
using Serilog.Core;
using Serilog.Events;
using System.Collections.Generic;

namespace LoggerDemo.Loggers
{
    public class HttpClientLogDatabaseSink : ILogEventSink
    {
        private readonly HttpClientLogRepository _httpClientLogRepository;

        public HttpClientLogDatabaseSink(HttpClientLogRepository httpClientLogRepository)
        {
            _httpClientLogRepository = httpClientLogRepository;
        }

        public void Emit(LogEvent logEvent)
        {
            var httpClientLog = new HttpClientLog
            {
                Timestamp = logEvent.Properties.GetValueOrDefault("Timestamp")?.ToString().Trim('"'),
                Method = logEvent.Properties.GetValueOrDefault("Method")?.ToString().Trim('"'),
                Endpoint = logEvent.Properties.GetValueOrDefault("Endpoint")?.ToString().Trim('"'),
                RequestHeaders = logEvent.Properties.GetValueOrDefault("RequestHeaders")?.ToString().Trim('"'),
                RequestBody = logEvent.Properties.GetValueOrDefault("RequestBody")?.ToString().Trim('"'),
                StatusCode = logEvent.Properties.GetValueOrDefault("StatusCode")?.ToString().Trim('"'),
                ResponseHeaders = logEvent.Properties.GetValueOrDefault("ResponseHeaders")?.ToString().Trim('"'),
                ResponseBody = logEvent.Properties.GetValueOrDefault("ResponseBody")?.ToString().Trim('"'),
                ExecutionTime = double.Parse(logEvent.Properties.GetValueOrDefault("ExecutionTime")?.ToString() ?? "0")
            };

            _httpClientLogRepository.Create(httpClientLog);
        }
    }
}
