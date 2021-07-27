namespace SeoEvaluation.Api.DTOs
{
    public class CustomException
    {
        public CustomException(string errorCode, string message = null, string details = null)
        {
            ErrorCode = errorCode;
            Message = message;
            Details = details;
        }

        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
