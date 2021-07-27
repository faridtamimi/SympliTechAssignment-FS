using System;
using System.Runtime.Serialization;

namespace SeoEvaluation.Entities.Exceptions
{
    public abstract class AppBaseException : Exception
    {
        public abstract string ErrorCode { get; }
        public AppBaseException()
        {
        }

        protected AppBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AppBaseException(string message) : base(message)
        {
        }

        public AppBaseException(string message, Exception innerException) : base(message, innerException)
        {
        }


    }
}
