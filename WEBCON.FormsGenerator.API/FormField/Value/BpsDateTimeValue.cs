using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField.Value
{
    internal class BpsDateTimeValue : DateTimeValue
    {
        public BpsDateTimeValue()
        {
        }

        public BpsDateTimeValue(object value) : base(value)
        {
        }

        public override DateTime? SetDateTimeValue(object value)
        {
            if (value == null) return null;
            if (!DateTime.TryParse(value.ToString(), out DateTime date))
                throw new FormFieldValueConversionException($"{value} is not correct value for date field type");

            return date;
        }
    }
}
