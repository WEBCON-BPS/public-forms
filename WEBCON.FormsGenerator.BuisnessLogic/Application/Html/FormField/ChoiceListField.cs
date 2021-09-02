using System.Collections.Generic;
using System.Linq;
using System.Text;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{
    internal class ChoiceListField : HtmlChoiceField
    {
        public ChoiceListField(FieldValue fieldValue) : base(fieldValue)
        {
            if (!(fieldValue is ChoiceListValue))
                throw new FormFieldValueFormatException("Passed field value is not correct choice list value");
        }
        public override string CreateInput()
        {
            if (!(Value?.Value is IEnumerable<ChoiceValue> values)) return "";
            StringBuilder dropdownBuilder = new StringBuilder();
            dropdownBuilder.Append(@$"<select data-bpsname=""{Name}"" name=""{Guid}"" id=""{Guid}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>");
            foreach (ChoiceValue value in values)
            {
                dropdownBuilder.AppendLine(CreateChoice(value));
            }
            dropdownBuilder.Append("</select>");
            return dropdownBuilder.ToString();
        }
        public override string CreateChoice(object value)
        {
            if (!(value is ChoiceValue choice)) return "";
            return @$"<option id =""{Guid}"" value=""{choice.Id + "#" + choice.Name}"">{choice.Name}</option>";
        }

        public override string UpdateChoicesOnFormContent(string formContent)
        {
            return base.UpdateChoicesOnFormContent<ChoiceValue>(formContent, $"//div[@id='group-{Guid}']/select", $"//select[@id='{Guid}']", "option", (value) => {if (value == null) return null; if(value is IEnumerable<ChoiceValue> choice) return choice.Select(x => new KeyValuePair<ChoiceValue,string>(x, x.Id + "#" + x.Name)).ToArray(); return null; });
        }
    }
}
