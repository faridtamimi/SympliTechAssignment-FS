using SeoEvaluation.Entities.Exceptions;

namespace SeoEvaluation.Api.DTOs
{
    public class ValidationException : AppBaseException
    {
        public ValidationException(string message) : base(message)
        {
            
        }
        public override string ErrorCode => "Err_003";
    }
}
