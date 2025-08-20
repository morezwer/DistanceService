using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DistanceService.Adapters.Middleware;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private const string _contentType = "application/json";
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex).ConfigureAwait(false);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            HttpRequestException => (int)HttpStatusCode.ServiceUnavailable,
            _ => (int)HttpStatusCode.InternalServerError
        };

        _logger.LogError(exception, "Произошло необработанное исключение");

        context.Response.ContentType = _contentType;
        context.Response.StatusCode = statusCode;

        var response = new { message = exception.Message };

        var json = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(json);
    }
}