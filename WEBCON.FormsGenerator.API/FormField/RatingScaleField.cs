using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField
{
    internal class RatingScaleField : BpsBaseField
    {
        public RatingScaleField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is RatingScaleValue))
                throw new FormFieldValueFormatException("Passed field value is not correct rating scale value");
        }

        public override object ValueToBps()
        {
            if (Value.Value == null) return null;
            int[] array = (int[])Value.Value;
            return (array.Length > 0) ? array[0] : 0;
        }
    }
}
