using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model
{
    public class FormContentField : Entity
    {
        public FormContentField()
        {

        }
        public FormContentField(Form form, BpsFormField bpsFormField, string name, string customName)
        {
            if (string.IsNullOrEmpty(name))
                throw new ApplicationArgumentException("Form content field - metadata name is required");
        
            Form = form ?? throw new ApplicationArgumentException("Form content field - form has to be provided");
            BpsFormField = bpsFormField ?? throw new ApplicationArgumentException("Form content field - form BPS field association is required");
            Guid = Guid.NewGuid();
            Name = name;
            SetCustomName(customName);
        }
        public Form Form { get; protected set; }
        public BpsFormField BpsFormField { get; protected set; }
        public string Name { get; protected set; }
        public string CustomName { get; protected set; }
        public string CustomRequiredWarningText { get; protected set; }
        public bool IsRequired { get; protected set; }
        public bool AllowMultipleValue { get; protected set; }
        public void SetCustomWarningText(string customRequiredWarningText)
        {
            CustomRequiredWarningText = customRequiredWarningText;
        }
        public void SetCustomName(string customName)
        {
            CustomName = customName;
        }
        public void SetIsRequired(bool isRequired)
        {
            IsRequired = isRequired;
        }
        public void SetAllowMultipleValue(bool allowMultipleValue)
        {
            AllowMultipleValue = allowMultipleValue;
        }
    }
}
