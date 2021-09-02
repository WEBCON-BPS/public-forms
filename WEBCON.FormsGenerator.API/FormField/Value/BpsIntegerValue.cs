using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField.Value
{
    internal class BpsIntegerValue : IntegerValue
    {
        public BpsIntegerValue()
        {
        }

        public BpsIntegerValue(object value) : base(value)
        {
        }

        public override int SetIntegerValue(object value)
        {
            return GetInteger(value);
        }
    }
}
