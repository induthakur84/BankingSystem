// Namespace: shared common project, so API, Services and Repository layers can all use this exception.
using System;

namespace ProjectCommonCode.Exceptions
{
    /// <summary>
    /// Custom exception thrown when a requested resource does not exist.
    /// Example: user with ID 5 not found, product not found, order not found.
    /// A global exception middleware converts this into an HTTP 404 (Not Found) response.
    /// </summary>
    public class NotFoundException : Exception   // Inherits from the built-in Exception class,
                                                 // so it can be thrown and caught like any exception.
    {
        // Constructor: accepts an error message that explains what was not found.
        // Example: throw new NotFoundException("User with ID 5 was not found.");
        //
        // ": base(message)" passes the message to the parent Exception class,
        // so it becomes available through the "ex.Message" property.
        public NotFoundException(string message) : base(message) { }
        // Empty body { } because the base class already stores the message.
        // No extra logic is needed here.
    }
}