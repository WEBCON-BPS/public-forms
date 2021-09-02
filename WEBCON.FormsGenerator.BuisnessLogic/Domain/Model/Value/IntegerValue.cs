using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value
{
    public abstract class IntegerValue : FieldValue
    {
        protected IntegerValue()
        {
        }

        protected IntegerValue(object value) : base(value)
        {
        }

        public override FormFieldType Type => FormFieldType.Integer;
        public abstract int SetIntegerValue(object value);
        public override object SetValue(object value)
        {
            return SetIntegerValue(value);
        }
        protected int GetInteger(object value)
        {
            if (value == null) return 0;
            string svalue = value.ToString();
            if (string.IsNullOrEmpty(svalue)) return 0;
            if (int.TryParse(svalue, out int result))
                return result;

            throw new FormFieldValueConversionException($"{value} is not correct value for integer field type");
        }
    }
}
