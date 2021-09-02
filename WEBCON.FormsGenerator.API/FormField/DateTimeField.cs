using System;
using System.Text.Json;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField
{
    internal class DateTimeField : BpsBaseField
    {
        public DateTimeField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is DateTimeValue))
                throw new FormFieldValueFormatException("Passed field value is not correct date value");
        }

        public override object ValueToBps()
        {
            if (Value.Value == null) return null;
            return JsonSerializer.Serialize(DateTime.Parse(Value.Value.ToString()));
        }
    }
}
