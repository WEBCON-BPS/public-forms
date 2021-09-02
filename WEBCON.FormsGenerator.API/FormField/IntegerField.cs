using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField
{
    internal class IntegerField : BpsBaseField
    {
        public IntegerField(FieldValue fieldValue) : base(fieldValue)
        {
            if(!(fieldValue is IntegerValue))
                throw new FormFieldValueFormatException("Passed field value is not correct integer value");
        }
    }
}
