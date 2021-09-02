using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField
{
    internal class EmailField : BpsBaseField
    {
        public EmailField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is EmailValue))
                throw new FormFieldValueFormatException("Passed field value is not correct email value");
        }
    }
}
