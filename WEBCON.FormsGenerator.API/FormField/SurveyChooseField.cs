using System.Collections.Generic;
using System.Linq;
using WEBCON.FormsGenerator.API.ApiModels;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField
{
    internal class SurveyChooseField : BpsBaseField
    {
        public SurveyChooseField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is SurveyChooseValue))
                throw new FormFieldValueFormatException("Passed field value is not correct survey choose value");
        }

        public override object ValueToBps()
        {
            if (!(Value.Value is IEnumerable<ChoiceValue> result)) return Value.Value;
            return new StartElementValueChoices() { choices = result.Select(x => new StartElementValueChoice { id = x.Id, name = x.Name }).ToArray() };
        }
    }
}
