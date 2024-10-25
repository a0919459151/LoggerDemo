using System;
using Microsoft.AspNetCore.Http;

namespace LoggerDemo.Excptions
{
    /// <summary>
    /// ApiException, 系統明確定義的錯誤情境
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// http 狀態碼
        /// </summary>
        public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string ErrorCode { get; set; }

        // Ctor for ErrorCode
        public ApiException(string errorCode, string errorMessage)
            : base(errorMessage)
        {
            ErrorCode = errorCode;
        }
    }
}
