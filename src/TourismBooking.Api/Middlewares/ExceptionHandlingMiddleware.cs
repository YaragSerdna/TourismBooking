using System.Net;
using System.Text.Json;

namespace TourismBooking.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invoca el siguiente middleware en la cadena y maneja cualquier excepción no controlada que ocurra durante la ejecución.
        /// </summary>
        /// <param name="context">Contexto de la solicitud HTTP.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error no controlado: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Maneja una excepción no controlada y devuelve una respuesta HTTP con el error correspondiente.
        /// </summary>
        /// <param name="context">Contexto de la solicitud HTTP.</param>
        /// <param name="exception">La excepción no controlada.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                ArgumentException argEx => (HttpStatusCode.BadRequest, argEx.Message),
                InvalidOperationException invEx => (HttpStatusCode.BadRequest, invEx.Message),
                KeyNotFoundException keyEx => (HttpStatusCode.NotFound, keyEx.Message),
                _ => (HttpStatusCode.InternalServerError, "Ha ocurrido un error interno en el servidor.")
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
