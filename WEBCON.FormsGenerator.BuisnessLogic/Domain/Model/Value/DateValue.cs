using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value
{
    public abstract class DateValue : FieldValue
    {
        protected DateValue()
        {
        }

        protected DateValue(object value) : base(value)
        {
        }
        public override FormFieldType Type => FormFieldType.Date;
        public abstract DateTime? SetDateValue(object value);
        public override object SetValue(object value)
        {
            return SetDateValue(value);
        }
        protected DateTime? GetDate(object value)
        {
            if (value == null) return null;
            string svalue = value.ToString();
            if (string.IsNullOrEmpty(svalue)) return null;

            if (!DateTime.TryParse(svalue, out DateTime result))
                throw new FormFieldValueConversionException($"{value} is not correct date value");

            return result;
        }
    }
}
