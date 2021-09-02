using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model
{
    public class BpsFormField : BpsEntity
    {
        public BpsFormField()
        {
        }
        public BpsFormField(Guid guid, string name, FormFieldType type, BpsFormType formType) : base(guid, name)
        {
            Validate(type, formType);
            Type = type;
            BPSFormType = formType;
        }
        public FormFieldType Type { get; protected set; }
        public bool IsRequired { get; set; }
        public bool IsReadonly { get; set; }
        public virtual BpsFormType BPSFormType { get; protected set; }
        private void Validate(FormFieldType type, BpsFormType formType)
        {
            if (type == FormFieldType.Undefined)
                throw new BpsEntityArgumentException("Cannot add field with an undefined type");
            if (formType == null)
                throw new BpsEntityArgumentException("Form field has to be associated with form");
        }

        public void Update(FormFieldType type, string name)
        {
            if (type == FormFieldType.Undefined)
                throw new BpsEntityArgumentException("Cannot add field with an undefined type");

            SetName(name);
        }
    }
}