using FluentValidation;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MantenimientoPresentation.Middlewares
{
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
            {
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogError(exception, "No se pudo manejar la excepción porque la respuesta ya había comenzado.");
                context.Abort();
                return;
            }

            var (status, message, validationErrors) = exception switch
            {
                ValidationException validationException =>
                    ((int)HttpStatusCode.BadRequest,
                     "Los datos enviados no son válidos.",
                     validationException.Errors.Select(error => error.ErrorMessage).Distinct().ToArray()),
                ArgumentException =>
                    ((int)HttpStatusCode.BadRequest, "Los datos enviados no son válidos.", Array.Empty<string>()),
                KeyNotFoundException =>
                    ((int)HttpStatusCode.NotFound, "No se encontró el recurso solicitado.", Array.Empty<string>()),
                InvalidOperationException =>
                    ((int)HttpStatusCode.Conflict, "No se pudo completar la operación solicitada.", Array.Empty<string>()),
                _ =>
                    ((int)HttpStatusCode.InternalServerError,
                     "Ocurrió un error interno en el servidor.",
                     Array.Empty<string>())
            };

            if (status >= (int)HttpStatusCode.InternalServerError)
                _logger.LogError(exception, "Error no controlado al procesar {Method} {Path}.", context.Request.Method, context.Request.Path);
            else
                _logger.LogWarning(exception, "Solicitud inválida al procesar {Method} {Path}.", context.Request.Method, context.Request.Path);

            context.Response.Clear();
            context.Response.StatusCode = status;
            context.Response.Headers["Cache-Control"] = "no-store";

            var acceptHeader = context.Request.Headers.Accept.ToString();
            if (acceptHeader.Contains("text/html", StringComparison.OrdinalIgnoreCase)
                && !acceptHeader.Contains("application/json", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                var title = status >= 500 ? "Error del servidor" : "No se pudo procesar la solicitud";
                var encodedMessage = HtmlEncoder.Default.Encode(message);
                await context.Response.WriteAsync(
                    $"<!doctype html><html lang=\"es\"><head><meta charset=\"utf-8\"><title>{title}</title></head>" +
                    $"<body><main><h1>{title}</h1><p>{encodedMessage}</p></main></body></html>");
                return;
            }

            context.Response.ContentType = "application/json; charset=utf-8";
            var errorResponse = new Dictionary<string, object>
            {
                ["success"] = false,
                ["statusCode"] = status,
                ["message"] = message
            };

            if (validationErrors.Length > 0)
                errorResponse["errors"] = validationErrors;

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}