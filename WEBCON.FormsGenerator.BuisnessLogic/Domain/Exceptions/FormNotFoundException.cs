using System;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions
{
    public class FormNotFoundException : ApplicationException
    {
        public FormNotFoundException() : base()
        {

        }
        public FormNotFoundException(string message) : base(message)
        {
        }
        public FormNotFoundException(string message, Exception innerException)
           : base(message, innerException)
        {
        }
    }
}
