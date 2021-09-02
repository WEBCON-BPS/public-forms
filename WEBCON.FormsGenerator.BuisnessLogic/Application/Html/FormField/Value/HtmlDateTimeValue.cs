using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value
{
    /// <summary>
    /// HTML implementation for get datetime value
    /// </summary>
    public class HtmlDateTimeValue : DateTimeValue
    {
        /// <param name="value">HTML datetime</param>
        public HtmlDateTimeValue(object value) : base(value)
        {
        }

        public override DateTime? SetDateTimeValue(object value)
        {
            return GetDateTime(value);
        }
    }
}
