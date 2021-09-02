using System.Collections.Generic;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value
{
    public abstract class ChoiceListValue : FieldValue
    {
        protected ChoiceListValue()
        {
        }

        protected ChoiceListValue(object value) : base(value)
        {
        }
        public override FormFieldType Type => FormFieldType.ChoiceList;
        public abstract List<ChoiceValue> SetChoiceListValue(object value);
        public override object SetValue(object value)
        {
            return SetChoiceListValue(value);
        }
    }
}
