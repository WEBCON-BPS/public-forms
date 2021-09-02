using System;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions
{
    public class BpsClientDataException : ApplicationException
    {
        public BpsClientDataException() : base()
        {

        }
        public BpsClientDataException(string message) : base (message)
        { 
        }
        public BpsClientDataException(string message, Exception innerException)
           : base(message, innerException)
        { }
    }
}
