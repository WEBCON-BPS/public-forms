using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value
{
    /// <summary>
    /// Html implementation for get integer value
    /// </summary>
    public class HtmlIntegerValue : IntegerValue
    {
        public HtmlIntegerValue()
        {
        }
        /// <param name="value">HTML integer value</param>
        public HtmlIntegerValue(object value) : base(value)
        {
        }

        public override int SetIntegerValue(object value)
        {
            return GetInteger(value);
        }
    }
}
