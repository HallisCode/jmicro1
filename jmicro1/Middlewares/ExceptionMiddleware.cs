using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace jmicro1.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<RequestDelegate> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<RequestDelegate> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception: exception,
                    message: "Global handling exceptions in middlewares pipeline."
                );
            }
        }
    }
}