using LoggerDemo.Loggers;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;
using LoggerDemo.DbEntities;
using System.Text.Json;
using LoggerDemo.Extensions;

namespace LoggerDemo.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Ignore swagger endpoints
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            var apiLogger = LoggerManager.ApiLogger;

            var requestBody = await ReadRequestBodyAsync(context);

            var log =  new ApiLog
            {
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                Method = context.Request.Method,
                Endpoint = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}",
                RequestHeaders = JsonSerializer.Serialize(context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())),
                RequestBody = await ReadRequestBodyAsync(context),
                StatusCode = null,
                ResponseHeaders = null,
                ResponseBody = null,
                ExecutionTime = 0L
            };

            var stopwatch = Stopwatch.StartNew();
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                log.StatusCode = context.Response.StatusCode.ToString();
                log.ResponseHeaders = JsonSerializer.Serialize(context.Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()));
                log.ResponseBody = await ReadResponseBodyAsync(context.Response);

                stopwatch.Stop();
                log.ExecutionTime = stopwatch.ElapsedMilliseconds;

                await responseBody.CopyToAsync(originalBodyStream); // Copy the response back to the original stream

                apiLogger.WriteCustomDbSinkLog(log);
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset stream position for further reading
                return body;
            }
        }

        private async Task<string> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string body = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin); // Reset stream position for further reading
            return body;
        }
    }
}
