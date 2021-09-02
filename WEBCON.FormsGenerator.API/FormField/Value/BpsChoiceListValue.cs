using System.Collections.Generic;
using System.Linq;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField.Value
{
    internal class BpsChoiceListValue : ChoiceListValue
    {
        public BpsChoiceListValue()
        {
        }

        public BpsChoiceListValue(object value) : base(value)
        {
        }

        public override List<ChoiceValue> SetChoiceListValue(object value)
        {
            if (value == null) return null;
            ApiModels.FormFieldValueData[] values = value as ApiModels.FormFieldValueData[];
            if (values == null) throw new FormFieldValueConversionException($"{value} is not correct value for choice list field type");

            return values.Select(x => new ChoiceValue { Id = x.id, Name = x.name }).ToList();
        }
    }
}
