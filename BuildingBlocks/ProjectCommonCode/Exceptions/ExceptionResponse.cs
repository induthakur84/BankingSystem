namespace ProjectCommonCode
{
    public class ExceptionResponse
    {
        public string Message { get; set; }
        public string? Details { get; set; }

        public ExceptionResponse(string message, string? details = null)
        {
            Message = message;
            Details = details;
        }
    }
}
