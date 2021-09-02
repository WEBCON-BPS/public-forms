using System.Collections.Generic;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value
{
    public abstract class SurveyChooseValue : FieldValue
    {
        protected SurveyChooseValue()
        {
        }
        protected SurveyChooseValue(object value) : base(value)
        {
        }
        public override FormFieldType Type => FormFieldType.SurveyChoose;

        public abstract List<ChoiceValue> SetSurveyChooseValue(object value);
        public override object SetValue(object value)
        {
            return SetSurveyChooseValue(value);
        }
    }
}
