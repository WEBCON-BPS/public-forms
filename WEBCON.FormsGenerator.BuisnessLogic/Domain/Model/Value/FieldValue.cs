using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model
{
    //Base abstract class for form field values.
    public abstract class FieldValue
    {
        public FieldValue()
        {

        }
        public FieldValue(object value)
        {
            Value = SetValue(value);
        }
        public abstract FormFieldType Type { get; }
        public object Value { get; protected set; }
        /// <summary>
        /// Set property Value for FieldValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract object SetValue(object value);

    }
}
