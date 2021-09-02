using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{
    internal class DateField : HtmlBaseField
    {
        public DateField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is DateValue))
                throw new FormFieldValueFormatException("Passed field value is not correct date value");
        }

        public override string CreateInput()
        {
            return $@"<input type=""text"" data-bpsname=""{Name}"" date-control=""true"" id=""{Guid}"" name=""{Guid}"" value=""{Value?.Value}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>";
        }
    }
}
