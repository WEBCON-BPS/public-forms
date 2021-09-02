using System.Collections.Generic;
using System.Linq;
using WEBCON.FormsGenerator.API.ApiModels;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField
{
    internal class ChoiceListField : BpsBaseField
    {
        public ChoiceListField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is ChoiceListValue))
                throw new FormFieldValueFormatException("Passed field value is not correct choice list value");
        }

        public override object ValueToBps()
        {
            if (!(Value.Value is IEnumerable<ChoiceValue> result)) return Value.Value;
            if (!result.Any()) return Value.Value;
            return result.Select(x => new StartElementValueChoice { id = x.Id, name = x.Name }).First();
        }
    }
}
