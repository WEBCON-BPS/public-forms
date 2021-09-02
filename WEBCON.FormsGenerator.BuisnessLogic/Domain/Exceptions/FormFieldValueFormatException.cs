using System;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions
{
    public class FormFieldValueFormatException : ApplicationException
    {
        public FormFieldValueFormatException(string message) : base(message)
        {
        }
        public FormFieldValueFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
