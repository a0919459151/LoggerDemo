using LoggerDemo.DbEntities;

namespace LoggerDemo.Extensions
{
    public static class LoggerExtensions
    {
        // Write Custom DbSink Log
        public static void WriteCustomDbSinkLog<T>(this Serilog.ILogger logger, T log)
        {
            if (log is ApiLog apiLog)
            {
                logger.Information(
                    "Timestamp: {Timestamp}, Method: {Method}, Endpoint: {Endpoint}, RequestHeaders = {RequestHeaders}, RequestBody = {RequestBody}, StatusCode = {StatusCode}, ResponseHeaders = {ResponseHeaders}, ResponseBody = {ResponseBody}, ExecutionTime = {ExecutionTime}",
                    apiLog.Timestamp,
                    apiLog.Method,
                    apiLog.Endpoint,
                    apiLog.RequestHeaders,
                    apiLog.RequestBody,
                    apiLog.StatusCode,
                    apiLog.ResponseHeaders,
                    apiLog.ResponseBody,
                    apiLog.ExecutionTime);
            }
            else if (log is HttpClientLog httpClientLog)
            {
                logger.Information(
                    "[Timestamp: {Timestamp}], Method: {Method}, Endpoint: {Endpoint}, RequestHeaders = {RequestHeaders}, RequestBody = {RequestBody}, StatusCode = {StatusCode}, ResponseHeaders = {ResponseHeaders}, ResponseBody = {ResponseBody}, ExecutionTime = {ExecutionTime}",
                    httpClientLog.Timestamp,
                    httpClientLog.Method,
                    httpClientLog.Endpoint,
                    httpClientLog.RequestHeaders,
                    httpClientLog.RequestBody,
                    httpClientLog.StatusCode,
                    httpClientLog.ResponseHeaders,
                    httpClientLog.ResponseBody,
                    httpClientLog.ExecutionTime);
            }
            else
            {
                // if type no map, ignore
            }
        }
    }
}
