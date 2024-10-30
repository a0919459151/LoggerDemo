using LoggerDemo.DbEntities;
using LoggerDemo.Extensions;
using LoggerDemo.Loggers;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoggerDemo.Clients.Common
{
    public class HttpClientLogHandler : DelegatingHandler 
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpClientLogger = LoggerManager.HttpClientLogger;

            var log = new HttpClientLog();

            try
            {
                log.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                log.Method = request.Method.ToString();
                log.Endpoint = request.RequestUri?.ToString();
                log.RequestHeaders = request.Content?.Headers.ToString() ?? "";
                log.RequestBody = request.Content != null ? await request.Content.ReadAsStringAsync() : "";

                var stopwatch = Stopwatch.StartNew();
                var response = await base.SendAsync(request, cancellationToken);
                stopwatch.Stop();

                log.StatusCode = ((int)response.StatusCode).ToString();
                log.ResponseHeaders = response.Content.Headers.ToString();
                log.ResponseBody = response.Content != null ? await response.Content.ReadAsStringAsync() : "";
                log.ExecutionTime = stopwatch.ElapsedMilliseconds;

                httpClientLogger.WriteCustomDbSinkLog(log);
                return response;
            }
            catch (Exception)
            {
                httpClientLogger.WriteCustomDbSinkLog(log);
                throw;
            }
        }
    }
}
