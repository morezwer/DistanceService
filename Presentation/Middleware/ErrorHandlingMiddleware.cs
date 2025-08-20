using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DistanceService.Presentation.Middleware;

/// <summary>
/// Универсальный промежуточный слой для обработки необработанных
/// исключений. Он логирует ошибки и возвращает стандартизированный
/// JSON‑ответ с корректным HTTP статусом. Таким образом, весь
/// обработчик ошибок находится в одном месте, что упрощает
/// сопровождение и обеспечивает единообразное поведение.
/// </summary>
public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Вызывается для обработки HTTP‑запроса. Оборачивает вызов
    /// следующего middleware в блок try/catch для перехвата
    /// исключений.
    /// </summary>
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
        // Выбираем HTTP‑статус на основе типа исключения. Здесь
        // отображаются основные известные ошибки. Все остальные
        // приводят к 500 Internal Server Error.
        var statusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            HttpRequestException => (int)HttpStatusCode.ServiceUnavailable,
            _ => (int)HttpStatusCode.InternalServerError
        };

        // Логируем исключение со всей подробной информацией. Журнал
        // настроен в appsettings.json. В production логирование должно
        // быть максимально информативным, но при этом не раскрывать
        // чувствительные данные пользователям.
        _logger.LogError(exception, "Произошло необработанное исключение");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var response = new { message = exception.Message };
        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}