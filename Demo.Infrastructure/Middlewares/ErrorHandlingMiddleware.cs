using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Core.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        private readonly IAppLogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, IAppLogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;

            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            if (exception is UnauthorizedAccessException)
            {
                code = HttpStatusCode.Unauthorized;
            }

            context.Response.StatusCode = (int)code;

            _logger.LogError(exception.Message);

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new { errorCode = code, error = exception.Message }));
        }
    }
}
