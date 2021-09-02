using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value
{
    public abstract class DateTimeValue : FieldValue
    {
        protected DateTimeValue()
        {
        }

        protected DateTimeValue(object value) : base(value)
        {
        }
        public override FormFieldType Type => FormFieldType.DateTime;
        public abstract DateTime? SetDateTimeValue(object value);
        public override object SetValue(object value)
        {
            return SetDateTimeValue(value);
        }
        protected DateTime? GetDateTime(object value)
        {
            if (value == null) return null;
            string svalue = value.ToString();
            if (string.IsNullOrEmpty(svalue)) return null;
            if (!DateTime.TryParse(svalue, out DateTime result))
                throw new FormFieldValueConversionException($"{value} is not correct datetime value");
            return result;
        }
    }
}
