using System;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions
{
    public class ApplicationArgumentException : ApplicationException
    {
        public ApplicationArgumentException() : base()
        {

        }
        public ApplicationArgumentException(string message) : base(message)
        {
        }
        public ApplicationArgumentException(string message, Exception innerException)
           : base(message, innerException)
        {
        }
    }
}
