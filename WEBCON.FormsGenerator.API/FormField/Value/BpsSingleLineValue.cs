using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField.Value
{
    internal class BpsSingleLineValue : SingleLineValue
    {
        public BpsSingleLineValue()
        {
        }

        public BpsSingleLineValue(object value) : base(value)
        {
        }

        public override string SetSingleLineValue(object value)
        {
            return value?.ToString();
        }
    }
}
