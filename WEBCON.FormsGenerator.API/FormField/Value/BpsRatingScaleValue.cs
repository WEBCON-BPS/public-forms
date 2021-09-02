using System.Linq;
using WEBCON.FormsGenerator.API.ApiModels;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField.Value
{
    internal class BpsRatingScaleValue : RatingScaleValue
    {
        public BpsRatingScaleValue()
        {
        }

        public BpsRatingScaleValue(object value) : base(value)
        {
        }

        public override int[] SetRatingScaleValue(object value)
        {
            if (value == null) return null;
            FormLayoutScaleValues values = value as FormLayoutScaleValues;
            if (values == null) 
                throw new FormFieldValueConversionException($"{value} is not correct value for rating scale field type");

            return Enumerable.Range(values.min, values.max).ToArray();
        }
    }
}
