using System.Collections.Generic;
using System.Linq;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField.Value
{
    internal class BpsSurveyChooseValue : SurveyChooseValue
    {
        public BpsSurveyChooseValue()
        {
        }

        public BpsSurveyChooseValue(object value) : base(value)
        {
        }

        public override List<ChoiceValue> SetSurveyChooseValue(object value)
        {
            if (value == null) return null;
            ApiModels.FormFieldValueData[] values = value as ApiModels.FormFieldValueData[];
            if (values == null) throw new FormFieldValueConversionException($"{value} is not correct value for survey choose field type");

            return values.Select(x => new ChoiceValue { Id = x.id, Name = x.name }).ToList();
        }
    }
}
