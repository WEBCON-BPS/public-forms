using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField
{
    internal class MultilineField : BpsBaseField
    {
        public MultilineField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is MultilineValue))
                throw new FormFieldValueFormatException("Passed field value is not correct text value");
        }
    }
}
