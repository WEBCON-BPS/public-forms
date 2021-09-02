using System.Globalization;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value
{
    public abstract class DecimalValue : FieldValue
    {
        protected DecimalValue()
        {
        }

        protected DecimalValue(object value) : base(value)
        {
        }

        public override FormFieldType Type => FormFieldType.Decimal;
        public abstract decimal SetDecimalValue(object value);
        public override object SetValue(object value)
        {
            return SetDecimalValue(value);
        }
        protected decimal GetDecimal(object value)
        {
            if (value == null) return 0;
            string svalue = value.ToString();
            if (string.IsNullOrEmpty(svalue)) return 0;
            if (decimal.TryParse(svalue, NumberStyles.Number, new NumberFormatInfo() { NumberDecimalSeparator = "," }, out decimal result))
                return result;
            if (decimal.TryParse(svalue, NumberStyles.Number, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out result))
                return result;

            throw new FormFieldValueConversionException($"{value} is not correct value for decimal field type");
        }
    }
}
