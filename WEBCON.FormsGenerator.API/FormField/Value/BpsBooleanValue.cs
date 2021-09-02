using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField.Value
{
    internal class BpsBooleanValue : BooleanValue
    {
        public BpsBooleanValue()
        {
        }

        public BpsBooleanValue(object value) : base(value)
        {
        }

        public override bool SetBooleanValue(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) return false;
            if(!(Boolean.TryParse(value.ToString(), out bool result)))
               throw new FormFieldValueConversionException($"{value} is not correct value for boolean field type");
            
            return result;
        }
    }
}
