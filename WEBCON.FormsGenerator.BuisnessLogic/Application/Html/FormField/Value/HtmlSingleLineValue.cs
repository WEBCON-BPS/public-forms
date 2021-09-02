using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value
{    
     /// <summary>
     /// Html implementation for get single line value
     /// </summary>
    public class HtmlSingleLineValue : SingleLineValue
    {
        public HtmlSingleLineValue()
        {
        }
        /// <param name="value">HTML single line value</param>

        public HtmlSingleLineValue(object value) : base(value)
        {
        }

        public override string SetSingleLineValue(object value)
        {
            return value?.ToString();
        }
    }
}
