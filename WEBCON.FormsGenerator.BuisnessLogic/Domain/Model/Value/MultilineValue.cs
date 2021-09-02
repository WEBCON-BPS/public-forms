using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value
{
    public abstract class MultilineValue : FieldValue
    {
        protected MultilineValue()
        {
        }

        protected MultilineValue(object value) : base(value)
        {
        }
        public override FormFieldType Type => FormFieldType.Multiline;
        public abstract string SetMultilineValue(object value);
        public override object SetValue(object value)
        {
            return SetMultilineValue(value);
        }
    }
}
