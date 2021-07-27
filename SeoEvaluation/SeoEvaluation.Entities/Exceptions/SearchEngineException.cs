namespace SeoEvaluation.Entities.Exceptions
{
    public class SearchEngineException : AppBaseException
    {
        public SearchEngineException(string engineName) : base($"Error in fetching data engine name = {engineName}")
        {
        }
        public override string ErrorCode => "Err_001";
    }
}
