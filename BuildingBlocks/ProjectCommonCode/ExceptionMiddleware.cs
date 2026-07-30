using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using ProjectCommonCode.Exceptions;

namespace ProjectCommonCode
{
    // Global Exception Middleware
    // this middleware catches all unhandled exceptions occurring anywhere
    // in the application and returns a standard JSON response format to the client.
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // pass the request to the next middleware
                await _next(context);
            }
            catch (Exception ex)
            {
                // if any exception occurs anywhere after this middleware, return proper JSON error response
                await HandleExceptionAsync(context, ex);
            }
        }

        // this method decides which http status code should be returned based on the exception type
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            string message;
            string? details = null;

            switch (exception)
            {
                case NotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    message = notFoundEx.Message;
                    break;

                case BadRequestException badRequestEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = badRequestEx.Message;
                    break;

                case ArgumentException argEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = argEx.Message;
                    break;

                case DbUpdateConcurrencyException dbUpdateConcurrencyEx:
                    statusCode = HttpStatusCode.Conflict;
                    message = "A concurrency conflict occurred while saving changes to the database.";
                    details = dbUpdateConcurrencyEx.InnerException == null
                        ? dbUpdateConcurrencyEx.Message
                        : dbUpdateConcurrencyEx.InnerException.Message;
                    break;

                case DbUpdateException dbUpdateEx:
                    statusCode = HttpStatusCode.Conflict;
                    message = "A database update error occurred.";
                    details = dbUpdateEx.InnerException == null
                        ? dbUpdateEx.Message
                        : dbUpdateEx.InnerException.Message;
                    break;

                default:
                    // Log unexpected error (Case 4)
                    _logger.LogError(exception, "An unexpected error occurred: {Message}", exception.Message);
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred. Please try again later.";
                    #if DEBUG
                    details = exception.StackTrace;
                    #endif
                    break;
            }

            var errorResponse = new ExceptionResponse(message, details);
            await BuildResponse(context, statusCode, errorResponse);
        }

        // this method is used to write the final JSON response and send it back to the client
        private static async Task BuildResponse(
            HttpContext context,
            HttpStatusCode statusCode,
            ExceptionResponse error
            )
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(error)
            );
        }
    }
}

