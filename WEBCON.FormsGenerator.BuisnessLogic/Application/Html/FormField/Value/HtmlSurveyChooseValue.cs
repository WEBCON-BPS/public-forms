using System.Collections.Generic;
using System.Text.RegularExpressions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value
{
    /// <summary>
    /// Html implementation for get survey choose value
    /// </summary>
    public class HtmlSurveyChooseValue : SurveyChooseValue
    {
        public HtmlSurveyChooseValue()
        {
        }
        /// <param name="value">HTML survey choose value</param>
        public HtmlSurveyChooseValue(object value) : base(value)
        {
        }

        public override List<ChoiceValue> SetSurveyChooseValue(object value)
        {
            if (value == null) return null;
            List<ChoiceValue> valueChoices = new List<ChoiceValue>();

            string[] varray = value.ToString().Split(",");
            foreach (string choose in varray)
            {
                Regex pattern = new Regex(@"(^[a-zA-Z0-9]{1,2})(\#)(.*$)");
                Match matched = pattern.Match(choose.ToString().Trim());
                if (!matched.Success || !matched.Groups.Count.Equals(4)) return null;               
                valueChoices.Add(new ChoiceValue
                {
                    Id = matched.Groups[1].ToString(),
                    Name = matched.Groups[3].ToString()
                });

            }
            return valueChoices;
        }
    }
}
