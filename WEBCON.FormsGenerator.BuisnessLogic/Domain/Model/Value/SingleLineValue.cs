using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value
{
    public abstract class SingleLineValue : FieldValue
    {
        protected SingleLineValue()
        {
        }
        protected SingleLineValue(object value) : base(value)
        {
        }
        public override FormFieldType Type => FormFieldType.SingleLine;
        public abstract string SetSingleLineValue(object value);
        public override object SetValue(object value)
        {
            return SetSingleLineValue(value);
        }
    }
}
