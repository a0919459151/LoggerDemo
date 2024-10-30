using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Net;
using LoggerDemo.Excptions;

namespace LoggerDemo.Middlewares
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Description);

    public class GlobalExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Ignore swagger endpoints
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await (ex switch
                {
                    // Handle app exception
                    AppException appException => GlobalExceptionHandler.HandleAppException(context, appException),

                    // Handle others exception
                    _ => GlobalExceptionHandler.HandleGenericException(context, ex)
                });
            }
        }
    }
}
