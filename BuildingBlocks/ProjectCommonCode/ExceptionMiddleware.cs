
// Used to access HttpContext, RequestDelegate and middleware-related classes.
using Microsoft.AspNetCore.Http;

// Used to handle Entity Framework Core database exceptions,
// such as concurrency conflicts and database update errors.
using Microsoft.EntityFrameworkCore;

// Used for logging errors and other application information.
using Microsoft.Extensions.Logging;

// Used to convert the error response object into a JSON string.
using Newtonsoft.Json;

// Contains our custom exception classes, such as NotFoundException
// and BadRequestException.
using ProjectCommonCode.Exceptions;

// Provides HTTP status codes such as 400, 404, 409 and 500.
using System.Net;

namespace ProjectCommonCode
{
    // ============================================================
    // GLOBAL EXCEPTION MIDDLEWARE
    // ============================================================
    //
    // This middleware catches unhandled exceptions that occur
    // in the downstream request pipeline.
    //
    // Instead of allowing an exception to produce an unhandled
    // server error, we convert it into a consistent JSON response.
    //
    // Example:
    // If a requested user does not exist, the API can return
    // HTTP 404 Not Found with an appropriate error message.
    //
    // IMPORTANT:
    // Register this middleware early in the request pipeline
    // so that it can catch exceptions from later middleware.
    public class ExceptionMiddleware
    {
        // RequestDelegate represents the next middleware in the pipeline.
        //
        // We call _next(context) to pass the current HTTP request
        // to the next middleware.
        private readonly RequestDelegate _next;

        // ILogger is used to record errors in application logs.
        //
        // We use this logger to help developers investigate
        // unexpected exceptions in the application.
        private readonly ILogger<ExceptionMiddleware> _logger;

        // Constructor:
        // ASP.NET Core automatically provides RequestDelegate and
        // ILogger through dependency injection when creating middleware.
        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            // Store the next middleware so we can pass requests forward.
            _next = next;

            // Store the logger so we can log unexpected errors.
            _logger = logger;
        }

        // Invoke() is called when a request reaches this middleware.
        //
        // HttpContext contains information about the current request
        // and response, including URL, headers, status code and body.
        //
        // ASP.NET Core invokes this method for each HTTP request.
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Pass the request to the next middleware.
                //
                // For example, the request may continue to routing,
                // authentication, authorization and the API controller.
                //
                // await allows us to wait for the downstream operation.
                // If an awaited downstream operation throws an exception,
                // control moves to the catch block below.
                await _next(context);
            }
            catch (Exception ex)
            {
                // If an exception occurs in the downstream pipeline,
                // catch it here instead of allowing it to go unhandled.
                //
                // Pass the exception to our handler, which decides
                // the appropriate HTTP status code and error message.
                await HandleExceptionAsync(context, ex);
            }
        }

        // ============================================================
        // HANDLE EXCEPTION
        // ============================================================
        //
        // This method examines the exception type and decides
        // which HTTP status code and message should be returned.
        //
        // For example:
        // NotFoundException  -> 404 Not Found
        // BadRequestException -> 400 Bad Request
        // Database conflict  -> 409 Conflict
        // Unexpected error   -> 500 Internal Server Error
        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            // Set the default status code to 500.
            //
            // This is used when an exception does not match
            // any of the specific cases in the switch statement.
            var statusCode = HttpStatusCode.InternalServerError;

            // This variable stores the main error message
            // that will be returned to the API client.
            string message;

            // Additional error information is optional.
            //
            // It starts as null and is populated only when needed.
            // The '?' indicates that the string can contain null.
            string? details = null;

            // Check the exception type to decide how to handle it.
            //
            // The switch statement uses pattern matching to identify
            // the exception type and access its properties.
            switch (exception)
            {
                // CASE 1: RESOURCE NOT FOUND
                //
                // This runs when the exception is a NotFoundException
                // or a derived type.
                case NotFoundException notFoundEx:

                    // HTTP 404 means the requested resource was not found.
                    statusCode = HttpStatusCode.NotFound;

                    // Use the message provided by the custom exception.
                    message = notFoundEx.Message;
                    break;

                // CASE 2: INVALID CLIENT REQUEST
                //
                // This runs when a BadRequestException is thrown.
                case BadRequestException badRequestEx:

                    // HTTP 400 indicates that the request is invalid.
                    statusCode = HttpStatusCode.BadRequest;

                    // Return the message describing the bad request.
                    message = badRequestEx.Message;
                    break;

                // CASE 3: ARGUMENT ERROR
                //
                // ArgumentException is commonly thrown when a method
                // receives an invalid argument.
                case ArgumentException argEx:

                    // Treat this exception as an HTTP 400 response.
                    statusCode = HttpStatusCode.BadRequest;

                    // Return the exception message.
                    message = argEx.Message;
                    break;

                // CASE 4: DATABASE CONCURRENCY CONFLICT
                //
                // DbUpdateConcurrencyException can occur when a database
                // record has changed or been deleted since it was read,
                // and EF Core detects a concurrency conflict.
                case DbUpdateConcurrencyException dbUpdateConcurrencyEx:

                    // HTTP 409 means the request conflicts with
                    // the current state of the resource.
                    statusCode = HttpStatusCode.Conflict;

                    // Return a general message that explains the conflict.
                    // This avoids exposing raw database exception details
                    // in the main message.
                    message =
                        "A concurrency conflict occurred while saving changes" +
                        " to the database.";

                    // Check whether the exception has an inner exception.
                    //
                    // InnerException may contain the underlying error
                    // that caused the database operation to fail.
                    //
                    // If there is no inner exception, use the original
                    // exception message instead.
                    details = dbUpdateConcurrencyEx.InnerException == null
                        ? dbUpdateConcurrencyEx.Message
                        : dbUpdateConcurrencyEx.InnerException.Message;

                    break;

                // CASE 5: GENERAL DATABASE UPDATE ERROR
                //
                // DbUpdateException may occur when EF Core cannot save
                // changes to the database.
                //
                // Examples include constraint violations or other
                // database update failures.
                case DbUpdateException dbUpdateEx:

                    // Return HTTP 409 Conflict for this database error
                    // according to this application's error-handling policy.
                    statusCode = HttpStatusCode.Conflict;

                    // Provide a general message to the API client.
                    message = "A database update error occurred.";

                    // If an inner exception exists, use its message
                    // as additional diagnostic information.
                    //
                    // Otherwise, use the original exception message.
                    details = dbUpdateEx.InnerException == null
                        ? dbUpdateEx.Message
                        : dbUpdateEx.InnerException.Message;

                    break;

                // CASE 6: UNEXPECTED EXCEPTION
                //
                // This case handles exceptions that do not match
                // any of the specific cases above.
                default:

                    // Log the complete exception.
                    //
                    // Passing 'exception' to LogError records the exception
                    // and its stack trace in the configured logging provider.
                    //
                    // {Message} is a structured logging placeholder.
                    // The exception message is stored as a log property.
                    _logger.LogError(
                        exception,
                        "An unexpected error occurred: {Message}",
                        exception.Message);

                    // Return HTTP 500 because the server encountered
                    // an unexpected error it could not handle.
                    statusCode = HttpStatusCode.InternalServerError;

                    // Return a general message instead of exposing
                    // internal implementation details to the client.
                    message =
                        "An unexpected error occurred. Please try again later.";

                    // DEBUG is a compilation symbol.
                    //
                    // When the application is compiled with DEBUG defined,
                    // include the stack trace in the details.
                    //
                    // The stack trace helps developers identify
                    // where the exception occurred.
                    //
                    // Avoid exposing stack traces in production because
                    // they can reveal internal application details.
#if DEBUG
                    details = exception.StackTrace;
#endif

                    break;
            }

            // Create a standard error response object.
            //
            // ExceptionResponse is our custom response class.
            // It receives the main message and optional details.
            //
            // Example response:
            // {
            //     "message": "User not found",
            //     "details": null
            // }
            var errorResponse = new ExceptionResponse(message, details);

            // Pass the status code and error response to BuildResponse().
            //
            // This method writes the final JSON response to the client.
            await BuildResponse(context, statusCode, errorResponse);
        }

        // ============================================================
        // BUILD AND SEND HTTP RESPONSE
        // ============================================================
        //
        // This method prepares the HTTP response and sends it back
        // to the client, such as Angular, Postman or another API.
        //
        // static means this method does not need an instance of
        // ExceptionMiddleware to be called.
        private static async Task BuildResponse(
            HttpContext context,
            HttpStatusCode statusCode,
            ExceptionResponse error)
        {
            // Convert the enum status code to its numeric HTTP value.
            //
            // Examples:
            // BadRequest = 400
            // NotFound = 404
            // Conflict = 409
            // InternalServerError = 500
            context.Response.StatusCode = (int)statusCode;

            // Tell the client that the response body contains JSON.
            //
            // This helps clients interpret the response correctly.
            context.Response.ContentType = "application/json";

            // Convert the error object into a JSON string.
            //
            // JsonConvert.SerializeObject() is provided by Newtonsoft.Json.
            //
            // WriteAsync() writes that JSON string into the HTTP response
            // body and sends it through the ASP.NET Core response pipeline.
            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(error)
            );
        }
    }
}
