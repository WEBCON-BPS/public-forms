using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{
    internal class EmailField : HtmlBaseField
    {
        public EmailField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is EmailValue))
                throw new FormFieldValueFormatException("Passed field value is not correct email value");
        }

        public override string CreateInput()
        {
            return CreateInput("email", Guid.ToString(), Guid.ToString(), Value?.Value);
        }
    }
}
