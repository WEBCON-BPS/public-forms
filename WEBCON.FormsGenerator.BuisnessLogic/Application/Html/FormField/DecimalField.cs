using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{
    internal class DecimalField : HtmlBaseField
    {
        public DecimalField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is DecimalValue))
                throw new FormFieldValueFormatException("Passed field value is not correct decimal value");
        }

        public override string CreateInput()
        {
            return $@"<input type=""number"" step="".01"" id=""{Guid}"" data-bpsname=""{Name}"" name=""{Guid}"" value=""{Value?.Value}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>";
        }
    }
}
