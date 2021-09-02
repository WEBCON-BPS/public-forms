using System.Collections.Generic;
using System.Linq;
using System.Text;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{
    internal class RatingScaleField : HtmlChoiceField
    {
        public RatingScaleField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is RatingScaleValue))
                throw new FormFieldValueFormatException("Passed field value is not correct rating scale value");
        }
        public override string CreateInput()
        {
            if (!(Value?.Value is int[] values)) return "";
            StringBuilder radioBuilder = new StringBuilder();
            foreach (int number in values)
            {
                radioBuilder.Append(CreateChoice(number));
            }
            return radioBuilder.ToString();
        }
        public override string UpdateChoicesOnFormContent(string formContent)
        {
            return base.UpdateChoicesOnFormContent<int>(formContent, $"//div[@id='group-{Guid}']","", "input", (value) => { if (value == null) return null; if (value is IEnumerable<int> choice) return choice.Select(x => new KeyValuePair<int, string>(x, x.ToString())).ToArray(); return null; });
        }
        public override string CreateChoice(object value)
        {
            if (!(value is int number)) return "";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div>");
            stringBuilder.AppendLine(@$"<input type=""radio"" id=""{Guid}"" data-bpsname=""{Name}"" name=""{Guid}"" value=""{number}"" {((number == 1) ? "checked" : "")} {FormParameters.IsRequired} {FormParameters.RequiredText} >");
            stringBuilder.AppendLine(@$"<label>{number}</label>");
            stringBuilder.Append("</div>");
            return stringBuilder.ToString();
        }
    }
}
