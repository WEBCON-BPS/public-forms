using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value
{
    /// <summary>
    /// Html implementation for get date value
    /// </summary>
    public class HtmlDateValue : DateValue
    {
        /// <param name="value">HTML date</param>
        public HtmlDateValue(object value) : base(value)
        {
        }

        public override DateTime? SetDateValue(object value)
        {
            return GetDate(value);
        }
    }
}
