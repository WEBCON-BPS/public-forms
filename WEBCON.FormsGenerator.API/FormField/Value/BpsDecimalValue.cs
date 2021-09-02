using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField.Value
{
    internal class BpsDecimalValue : DecimalValue
    {
        public BpsDecimalValue()
        {
        }

        public BpsDecimalValue(object value) : base(value)
        {
        }

        public override decimal SetDecimalValue(object value)
        {
            return GetDecimal(value);
        }
    }
}
