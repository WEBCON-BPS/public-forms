using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value
{
    /// <summary>
    /// Html implementation for get email value
    /// </summary>
    public class HtmlEmailValue : EmailValue
    {
        /// <param name="value">HTML email value</param>
        public HtmlEmailValue(object value) : base(value)
        {
        }

        public override string SetEmailValue(object value)
        {
            return GetEmail(value);
        }
    }
}
