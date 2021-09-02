using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{
    internal class IntegerField : HtmlBaseField
    {
        public IntegerField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is IntegerValue))
                throw new FormFieldValueFormatException("Passed field value is not correct integer value");
        }

        public override string CreateInput()
        {
            return CreateInput("number", Guid.ToString(), Guid.ToString(), Value?.Value);
        }
    }
}
