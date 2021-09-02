using System;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions
{
    public class BpsEntityArgumentException : ApplicationException
    {
        public BpsEntityArgumentException() : base()
        {

        }
        public BpsEntityArgumentException(string message) : base(message)
        {
        }
        public BpsEntityArgumentException(string message, Exception innerException)
           : base(message, innerException)
        {
        }
    }
}
