using LoggerDemo.DbEntities;
using LoggerDemo.Loggers;
using LoggerDemo.Repositories;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoggerDemo.Clients.Common
{
    public class HttpClientLogHandler : DelegatingHandler 
    {
        private readonly LoggerManager _loggerManager;
        private readonly HttpClientLogRepository _httpClientLogRepository;

        public HttpClientLogHandler(
            LoggerManager loggerManager,
            HttpClientLogRepository httpClientLogRepository)
        {
            _loggerManager = loggerManager;
            _httpClientLogRepository = httpClientLogRepository;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiLogger = _loggerManager.GetHttpClientLogger();

            //var a = await request.Content.ReadAsStringAsync();

            var log = new HttpClientLog();

            try
            {
                log.Timestamp = DateTime.Now.ToString();
                log.Method = request.Method.ToString();
                log.Endpoint = request.RequestUri?.ToString();
                log.RequestHeaders = request.Content?.Headers.ToString() ?? "";
                log.RequestBody = request.Content != null ? await request.Content.ReadAsStringAsync() : "";

                var stopwatch = Stopwatch.StartNew();
                var response = await base.SendAsync(request, cancellationToken);
                stopwatch.Stop();

                log.StatusCode = response.StatusCode.ToString();
                log.ResponseHeaders = response.Content.Headers.ToString();
                log.ResponseBody = response.Content != null ? await response.Content.ReadAsStringAsync() : "";
                log.ExecutionTime = stopwatch.ElapsedMilliseconds;

                WriteLog(apiLogger, log);

                return response;
            }
            catch (Exception)
            {
                WriteLog(apiLogger, log);
                throw;
            }
        }

        // apiLogger write log
        private void WriteLog(Serilog.ILogger apiLogger, HttpClientLog log)
        {
            apiLogger.Information(
                "Timestamp: {Timestamp}, Method: {Method}, Endpoint: {Endpoint}, RequestHeaders = {RequestHeaders}, RequestBody = {RequestBody}, StatusCode = {StatusCode}, ResponseHeaders = {ResponseHeaders}, ResponseBody = {ResponseBody}, ExecutionTime = {ExecutionTime}",
                log.Timestamp,
                log.Method,
                log.Endpoint,
                log.RequestHeaders,
                log.RequestBody,
                log.StatusCode,
                log.ResponseHeaders,
                log.ResponseBody,
                log.ExecutionTime);
        }
    }
}
