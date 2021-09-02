using System;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException() : base()
        {

        }
        public ApplicationException(string message) : base(message)
        {
        }
        public ApplicationException(string message, Exception innerException)
           : base(message, innerException)
        { }
    }
}
