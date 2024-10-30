using LoggerDemo.Excptions;
using LoggerDemo.Extensions;
using LoggerDemo.Loggers;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace LoggerDemo.Middlewares
{
    public static class GlobalExceptionHandler
    {
        public static async Task HandleAppException(HttpContext context, AppException exception)
        {
            var statusCode = exception.StatusCode;
            var errorCode = !string.IsNullOrEmpty(exception.ErrorCode) ? exception.ErrorCode : AppError.DefaultErrorCode;
            var errorMessage = !string.IsNullOrEmpty(exception.Message) ? exception.Message : AppError.DefaultErrorMessage;

            LoggerManager.AppLogger.Error(exception, "ErrorCode: {ErrorCode}, Message: {Message}", errorCode, errorMessage);

            await context.WriteResponse(
                statusCode,
                errorCode,
                errorMessage);
        }

        public static async Task HandleGenericException(HttpContext context, Exception exception)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var errorCode = AppError.DefaultErrorCode;
            var errorMessage = !string.IsNullOrEmpty(exception.Message) ? exception.Message : AppError.DefaultErrorMessage;

            LoggerManager.AppLogger.Error(exception, errorMessage);

            await context.WriteResponse(
                statusCode,
                errorCode,
                errorMessage);
        }
    }
}
