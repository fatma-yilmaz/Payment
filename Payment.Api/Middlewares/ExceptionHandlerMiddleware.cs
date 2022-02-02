using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Payment.Api.Core.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Payment.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (PaymentException exception)
            {
                _logger.LogError(exception.Message);
                var response = httpContext.Response;
                response.ContentType = "application/json";
                var responseModel = new ErrorDetails{Message = exception.Message,};
                response.StatusCode = (int)exception.StatusCode;

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                await HandleExceptionAsync(httpContext, exception);
            }
        }


        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                Message = $"Something went wrong! {exception.Message}"
            }.ToString());
        }
    }

    public class ErrorDetails
    {
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
