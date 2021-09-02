using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.API.FormField.Value
{
    internal class BpsEmailValue : EmailValue
    {
        public BpsEmailValue()
        {
        }

        public BpsEmailValue(object value) : base(value)
        {
        }

        public override string SetEmailValue(object value)
        {
            return GetEmail(value);
        }
    }
}
