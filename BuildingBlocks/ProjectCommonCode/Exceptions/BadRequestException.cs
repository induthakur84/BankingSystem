// Namespace: groups related classes together.
// "ProjectCommonCode" is a shared project, so API, Services and Repository layers can all use this exception.
using System;

namespace ProjectCommonCode.Exceptions
{
    /// <summary>
    /// Custom exception thrown when the client sends invalid or incorrect data.
    /// Example: missing email, wrong format, duplicate value, invalid ID.
    /// A global exception middleware converts this into an HTTP 400 (Bad Request) response.
    /// </summary>
    public class BadRequestException : Exception   // Inherits from the built-in Exception class,
                                                   // so it can be thrown and caught like any exception.
    {
        // Constructor: accepts an error message that explains what went wrong.
        // Example: throw new BadRequestException("Email is required.");
        //
        // ": base(message)" passes the message to the parent Exception class,
        // so it becomes available through the "ex.Message" property.
        public BadRequestException(string message) : base(message) { }
        // Empty body { } because the base class already stores the message.
        // No extra logic is needed here.
    }
}