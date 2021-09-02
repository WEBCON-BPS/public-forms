using System;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{ 
    internal class BooleanFormField : HtmlBaseField
    {
        public BooleanFormField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is BooleanValue))
                throw new FormFieldValueFormatException("Passed field value is not correct boolean value");
        }
        public override string CreateInput()
        {
            return @$"<input type=""checkbox"" data-bpsname=""{Name}"" id=""{Guid}"" name=""{Guid}""{(Convert.ToBoolean(Value.Value) ? " checked" : null)} {FormParameters.IsRequired} {FormParameters.RequiredText}>";
        }
    }
}
