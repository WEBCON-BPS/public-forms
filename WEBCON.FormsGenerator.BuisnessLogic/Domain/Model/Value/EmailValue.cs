using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value
{
    public abstract class EmailValue : FieldValue
    {
        protected EmailValue()
        {
        }

        protected EmailValue(object value) : base(value)
        {
        }

        public override FormFieldType Type => FormFieldType.Email;
        public abstract string SetEmailValue(object value);
        public override object SetValue(object value)
        {
            return SetEmailValue(value);
        }
        public string GetEmail(object value)
        {
            if (value == null) return null;
            string svalue = value.ToString();
            if (string.IsNullOrEmpty(svalue)) return svalue;
            try
            {
                var addr = new System.Net.Mail.MailAddress(svalue);
                return svalue;
            }
            catch
            {
                throw new FormFieldValueConversionException($"{value} is not correct value for email field type");

            }
        }
    }
}
