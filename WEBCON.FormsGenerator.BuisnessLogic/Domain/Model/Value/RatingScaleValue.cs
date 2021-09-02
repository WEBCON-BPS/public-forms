using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value
{
    public abstract class RatingScaleValue : FieldValue
    {
        protected RatingScaleValue()
        {
        }

        protected RatingScaleValue(object value) : base(value)
        {
        }
        public override FormFieldType Type => FormFieldType.RatingScale;
        public abstract int[] SetRatingScaleValue(object value);
        public override object SetValue(object value)
        {
            return SetRatingScaleValue(value);
        }
    }
}
