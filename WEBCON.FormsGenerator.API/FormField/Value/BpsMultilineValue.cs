using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField.Value
{
    internal class BpsMultilineValue : MultilineValue
    {
        public BpsMultilineValue()
        {
        }

        public BpsMultilineValue(object value) : base(value)
        {
        }
        public override string SetMultilineValue(object value)
        {
            return value?.ToString();
        }
    }
}
