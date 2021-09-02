using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Service
{
    public class FormFieldFactory : IFormFieldFactory
    {
        public FormFieldFactory(IFormFieldBuilder formFieldBuilder, IFormFieldValueBuilder formValueBuilder)
        {
            FormFieldBuilder = formFieldBuilder;
            FormValueBuilder = formValueBuilder;
        }

        public IFormFieldBuilder FormFieldBuilder { get; private set; }
        public IFormFieldValueBuilder FormValueBuilder { get; private set; }     
    }
}
