using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField
{
    internal class BooleanField : BpsBaseField
    {
        public BooleanField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is BooleanValue))
                throw new FormFieldValueFormatException("Passed field value is not correct boolean value");
        }
    }
}
