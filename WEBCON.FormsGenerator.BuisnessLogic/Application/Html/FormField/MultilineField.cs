using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{
    internal class MultilineField : HtmlBaseField
    {
        public MultilineField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is MultilineValue))
                throw new FormFieldValueFormatException("Passed field value is not correct text value");
        }

        public override string CreateInput()
        {
           return $@"<textarea id=""{Guid}"" data-bpsname=""{Name}"" name=""{Guid}"" value=""{Value?.Value}"" {FormParameters.IsRequired} {FormParameters.RequiredText}></textarea>";
        }
    }
}
