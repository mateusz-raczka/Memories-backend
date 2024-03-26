using Memories_backend.Utilities.Exceptions;
using System.Net;

namespace Memories_backend.Middlewares
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Description);

    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred.");

            ExceptionResponse response = exception switch
            {
                ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred. Error: " + exception.Message),
                KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The request key not found. Error: " + exception.Message),
                UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized. Error: " + exception.Message),
                ForbiddenException _ => new ExceptionResponse(HttpStatusCode.Forbidden, "Forbidden. Error: " + exception.Message),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later. Error: " + exception.Message)
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
