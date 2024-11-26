using Newtonsoft.Json;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Result;
using System.Net;

namespace SpendingTracker.API.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            ExceptionResponse response = exception switch
            {
                UnauthorizedAccessException => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
                ArgumentException ex => new ExceptionResponse(HttpStatusCode.BadRequest, ex.Message),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            }; ;

            context.Response.StatusCode = (int)response.StatusCode;



            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
