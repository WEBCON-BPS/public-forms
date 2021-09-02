using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value
{    
    /// <summary>    
    /// Html implementation for get boolean value
    /// </summary>
    public class HtmlRatingScaleValue : RatingScaleValue
    {
        public HtmlRatingScaleValue()
        {
        }
        /// <param name="value">HTML rating scale value</param>
        public HtmlRatingScaleValue(object value) : base(value)
        {
        }

        public override int[] SetRatingScaleValue(object value)
        {
            if (value == null) return null;
            string svalue = value.ToString();
            if (string.IsNullOrEmpty(svalue)) return null;
            if(!int.TryParse(svalue, out int result)) return null;
            return new int[] { result };
        }
    }
}
