// Namespace: shared common project, so API, Services and Repository layers can all use this class.
namespace ProjectCommonCode
{
    /// <summary>
    /// A standard shape for error responses sent back to the client.
    /// Instead of returning random error formats, every error from the API
    /// is wrapped in this class, so the client always gets the same JSON structure.
    ///
    /// Example JSON response:
    /// {
    ///   "message": "Email is required.",
    ///   "details": null
    /// }
    /// </summary>
    public class ExceptionResponse
    {
        // The main, user-friendly error message.
        // Example: "Email is required." or "Something went wrong."
        // This is always present.
        public string Message { get; set; }

        // Extra technical information about the error (optional).
        // The "?" means it can be null.
        // Example: stack trace or inner exception message.
        // Tip: send details only in the Development environment, never in Production,
        // because it can expose internal information to the client.
        public string? Details { get; set; }

        // Constructor: creates the response object.
        // "message" is required.
        // "details" is optional, and its default value is null,
        // so you can create the object with just a message:
        //     new ExceptionResponse("Email is required.");
        // or with both:
        //     new ExceptionResponse("Something went wrong.", ex.StackTrace);
        public ExceptionResponse(string message, string? details = null)
        {
            Message = message;   // Store the main message.
            Details = details;   // Store the optional details (null if not provided).
        }
    }
}