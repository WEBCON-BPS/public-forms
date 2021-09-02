using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField.Value
{
    internal class BpsDateValue : DateValue
    {
        public BpsDateValue()
        {
        }

        public BpsDateValue(object value) : base(value)
        {
        }

        public override DateTime? SetDateValue(object value)
        {
            return GetDate(value);
        }
    }
}
