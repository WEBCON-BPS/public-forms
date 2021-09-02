using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value
{
    public abstract class BooleanValue : FieldValue
    {
        protected BooleanValue()
        {
        }

        protected BooleanValue(object value) : base(value)
        {
        }
        public override FormFieldType Type => FormFieldType.Boolean;
        public abstract bool SetBooleanValue(object value);
        public override object SetValue(object value)
        {
            return SetBooleanValue(value);
        }
    }
}
