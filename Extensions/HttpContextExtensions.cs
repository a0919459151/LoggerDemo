using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoggerDemo.Extensions
{
    public static class HttpContextExtensions
    {
        // WriteResponse
        public static async Task WriteResponse(this HttpContext context, int statusCode, string errorCode, string errorMessage)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var responseObject = new
            {
                ErrorCode = errorCode,
                Message = errorMessage,
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(responseObject));
        }
    }
}
