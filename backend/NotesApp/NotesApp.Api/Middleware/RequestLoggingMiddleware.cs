using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace NotesApp.Api.Middleware;

/// <summary>
/// Middleware to log HTTP request duration and basic info.
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        
        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();
            var statusCode = context.Response.StatusCode;
            
            if (statusCode >= 400)
            {
                _logger.LogWarning("HTTP {Method} {Path} completed with status {StatusCode} in {ElapsedMilliseconds}ms",
                    context.Request.Method, context.Request.Path, statusCode, sw.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation("HTTP {Method} {Path} completed with status {StatusCode} in {ElapsedMilliseconds}ms",
                    context.Request.Method, context.Request.Path, statusCode, sw.ElapsedMilliseconds);
            }
        }
    }
}
