using System;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException() : base()
        {

        }
        public NotFoundException(string message) : base(message)
        {
        }
        public NotFoundException(string message, Exception innerException)
           : base(message, innerException)
        {
        }
    }
}
