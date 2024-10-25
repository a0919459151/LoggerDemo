using LoggerDemo.Excptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoggerDemo.Middlewares
{
    public static class GlobalExceptionHandler
    {
        public static async Task HandleApiException(HttpContext context, ApiException exception)
        {
            //logger.Error(exception, "Exception occurred");

            context.Response.StatusCode = exception.StatusCode;
            context.Response.ContentType = "application/json";

            var responseObject = new
            {
                ErrorCode = exception.ErrorCode,
                Message = exception.Message != null ? exception.Message : ApiError.DefaultErrorMessage,
            };

            await HttpResponseWritingExtensions.WriteAsync(context.Response, JsonSerializer.Serialize(responseObject));
        }

        public static async Task HandleGenericException(HttpContext context, Exception exception)
        {
            //logger.Error(exception, "Exception occurred");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var responseObject = new
            {
                ErrorCode = ApiError.DefaultErrorCode,  // "0"
                Message = exception.Message != null ? exception.Message : ApiError.DefaultErrorMessage,
            };

            await HttpResponseWritingExtensions.WriteAsync(context.Response, JsonSerializer.Serialize(responseObject));
        }
    }
}
