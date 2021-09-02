using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value
{    
     /// <summary>
     /// Html implementation for get multiline value
     /// </summary>
    public class HtmlMultilineValue : MultilineValue
    {
        /// <param name="value">HTML multiline value</param>
        public HtmlMultilineValue(object value) : base(value)
        {
        }
        public override string SetMultilineValue(object value)
        {
            return value?.ToString();
        }
    }
}
