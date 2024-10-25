using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using LoggerDemo.Excptions;

namespace LoggerDemo.Middlewares
{
    public class GlobalExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await (ex switch
                {
                    // Handle api exception
                    ApiException appException => GlobalExceptionHandler.HandleApiException(context, appException),

                    // Handle others exception
                    _ => GlobalExceptionHandler.HandleGenericException(context, ex)
                });
            }
        }
    }
}
