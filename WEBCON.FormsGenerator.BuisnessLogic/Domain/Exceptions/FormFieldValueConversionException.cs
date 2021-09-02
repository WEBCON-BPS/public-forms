using System;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions
{
    public class FormFieldValueConversionException : ApplicationException
    {
        public FormFieldValueConversionException(string message) : base(message)
        {
        }
        public FormFieldValueConversionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
