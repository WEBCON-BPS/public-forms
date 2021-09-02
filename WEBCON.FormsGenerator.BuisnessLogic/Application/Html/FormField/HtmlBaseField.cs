using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{
    abstract class HtmlBaseField : DTO.FormField
    {
        protected HtmlBaseField(FieldValue value) : base(value)
        {
        }
        public abstract string CreateInput();
        protected string CreateInput(string type, string name, string id, object value)
        {
            return @$"<input type=""{type}"" data-bpsname=""{Name}"" id=""{id}"" name=""{name}"" value=""{value??""}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>";
        }
    }
}
