using System.Collections.Generic;
using System.Text.RegularExpressions;
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
       
            Regex pattern = new Regex(@"(^[a-zA-Z0-9]*)(\#)(.*$)");
            Match matched = pattern.Match(value.ToString().Trim());
            if (!matched.Success || !matched.Groups.Count.Equals(4)) return null; 
            return new List<ChoiceValue>
            {
              new ChoiceValue{ Id = matched.Groups[1].Value.ToString(), Name =  matched.Groups[3].ToString() }
            };
        }
    }
}
