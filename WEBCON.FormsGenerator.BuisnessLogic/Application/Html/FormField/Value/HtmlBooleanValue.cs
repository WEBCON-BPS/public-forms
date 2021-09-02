using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value
{
    /// <summary>
    /// HTML implementation for get boolean value
    /// </summary>
    public class HtmlBooleanValue : BooleanValue
    {
        /// <param name="value">HTML boolean value</param>
        public HtmlBooleanValue(object value) : base(value)
        {
        }

        public override bool SetBooleanValue(object value)
        {
            if (value == null) return false;
            string svalue = value.ToString();
            return (svalue.ToLower().Equals("on") || svalue.ToLower().Equals("true"));
        }
    }
}
