using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Common.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log Request
            _logger.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path}");

            await _next(context); // Call the next middleware

            // Log Response
            _logger.LogInformation($"Response Status Code: {context.Response.StatusCode}");
        }
    }

}
