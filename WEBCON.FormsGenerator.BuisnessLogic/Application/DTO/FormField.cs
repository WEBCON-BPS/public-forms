using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.DTO
{
    public class FormField : BaseObject
    {
        public FormField()
        {

        }
        protected FormField(FieldValue value)
        {
            Value = value;
        }
        public bool IsRequired { get; set; }   
        public bool IsReadonly { get; set; }
        public bool AllowMultipleValues { get; set; }
        public FieldValue Value { get; protected set; }
        public FormFieldType Type { get; set; }
    }

}
