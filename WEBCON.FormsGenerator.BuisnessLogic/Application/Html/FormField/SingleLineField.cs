using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{
    internal class SingleLineField : HtmlBaseField
    {
        public SingleLineField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is SingleLineValue))
                throw new FormFieldValueFormatException("Passed field value is not correct text value");
        }

        public override string CreateInput()
        {
            return CreateInput("text", Guid.ToString(), Guid.ToString(), Value?.Value);
        }
    }
}
