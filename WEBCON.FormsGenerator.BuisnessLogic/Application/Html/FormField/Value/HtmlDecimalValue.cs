using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value
{    
     /// <summary>
     /// Html implementation for get decimal value
     /// </summary>
    public class HtmlDecimalValue : DecimalValue
    {
        /// <param name="value">HTML decimal value</param>
        public HtmlDecimalValue(object value) : base(value)
        {
        }

        public override decimal SetDecimalValue(object value)
        {
            return GetDecimal(value);
        }
    }
}
