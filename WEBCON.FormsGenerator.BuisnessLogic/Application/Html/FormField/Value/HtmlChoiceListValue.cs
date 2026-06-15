using System.Collections.Generic;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value
{
    /// <summary>
    /// HTML implementation for get choice list value
    /// </summary>
    public class HtmlChoiceListValue : ChoiceListValue
    {
        /// <param name="value">HTML choice list value</param>
        public HtmlChoiceListValue(object value) : base(value)
        {
        }

        public override List<ChoiceValue> SetChoiceListValue(object value)
        {
            if (value == null) return null;

            var str = value.ToString().Trim();
            var separatorIndex = str.IndexOf('#');

            if (separatorIndex < 0) return null;

            return new List<ChoiceValue>
            {
                new ChoiceValue { Id = str[..separatorIndex], Name = str[(separatorIndex + 1)..] }
            };
        }
    }
}
