using System.Collections.Generic;
using System.Linq;
using System.Text;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{
    internal class SurveyChooseField : HtmlChoiceField
    {
        public SurveyChooseField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is SurveyChooseValue))
                throw new FormFieldValueFormatException("Passed field value is not correct survey choose value");
        }

        public override string CreateInput()
        {
            if (!(Value?.Value is IEnumerable<ChoiceValue> values)) return "";
            StringBuilder multichoiceBuilder = new StringBuilder();
            multichoiceBuilder.Append(@$"<fieldset id=""{Guid}"" data-bpsname=""{Name}"" {FormParameters.AllowMultipleValues} name=""{Guid}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>");
            foreach (ChoiceValue value in values)
            {
                multichoiceBuilder.AppendLine(CreateChoice(value));
            }
            multichoiceBuilder.Append("</fieldset>");
            return multichoiceBuilder.ToString();
        }
        public override string CreateChoice(object value)
        {
            if (!(value is ChoiceValue choice)) return "";
            return @$"<div><input type=""checkbox"" id=""{Guid}"" name=""{Guid}"" value=""{choice.Id + "#" + choice.Name}""><label>{choice.Name}</label></div>";
        }
        public override string UpdateChoicesOnFormContent(string formContent)
        {
            return base.UpdateChoicesOnFormContent<ChoiceValue>(formContent, $"//div[@id='group-{Guid}']/fieldset", $"//fieldset[@id='{Guid}']", "input", (value) => { if (value == null) return null; if (value is IEnumerable<ChoiceValue> choice) return choice.Select(x => new KeyValuePair<ChoiceValue, string>(x, x.Id + "#" + x.Name)).ToArray(); return null; });
        }
    }
}
