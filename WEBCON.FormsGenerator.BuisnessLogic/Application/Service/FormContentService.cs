using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Service
{
    public class FormContentService : IFormContentService
    {
        private readonly IFormBuilder formBuilder;

        public FormContentService(IFormBuilder formBuilder)
        {
            this.formBuilder = formBuilder ?? throw new ApplicationArgumentException("Form builder instance not provided");
        }
        public FormContent CreateFormContent(IEnumerable<DTO.FormField> formFields, string contentTitle)
        {
            if (formFields == null)
                throw new ApplicationArgumentException("Form fields are not provided. Could not create form.");
            if (!formFields.Any())
                throw new ApplicationArgumentException("Form fields are not provided. Could not create form.");

            FormContent formBody = new FormContent();
            List<FormContentField> formContentFields = new List<FormContentField>();
            StringBuilder htmlForm = new StringBuilder();

            string formNameArea = formBuilder.CreateFormNameArea(FormParameters.ContentTitle);
            string formDescriptionArea = formBuilder.CreateFormDescriptionArea(FormParameters.ContentDescription);
            foreach (DTO.FormField field in formFields)
            {                
                var fieldBody = CreateFormContentInput(field);
                if (string.IsNullOrEmpty(fieldBody)) continue;

                var contentField = CreateFormContentField(field);
                formContentFields.Add(contentField);

                htmlForm.AppendLine(fieldBody);
            }
            string formWithMetadata = formBuilder.EmbedFieldsInForm(htmlForm.ToString());

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(formNameArea);
            stringBuilder.AppendLine(formDescriptionArea);
            stringBuilder.AppendLine(formWithMetadata);

            formBody.FormContentWithMetadata = stringBuilder.ToString();
            formBody.FormContentTransformed = formBuilder.TransformMetadataOnForm(formContentFields, stringBuilder.ToString(), contentTitle);
            formBody.ContentFields = formContentFields;
            return formBody;
        }
        public string CreateFormContentInput(DTO.FormField field)
        {
            if (field.IsReadonly) return "";
            string fieldNativeName = $"{FormParameters.FormContentFieldNamePrefix}{field.Guid}";
            string htmlField = formBuilder.CreateFormFieldBasedOnBpsField(field);
            if (string.IsNullOrEmpty(htmlField)) return "";

            string label = formBuilder.CreateLabel(fieldNativeName, "{" + fieldNativeName + "}");

            return formBuilder.EmbedInputsInGroup(new string[] { label, htmlField }, field.Guid.ToString());
        }
        private FormContentField CreateFormContentField(FormField field)
        {
            FormContentField contentField = new FormContentField
            {
                BpsFormField = field,
                CustomName = field.Name,
                Name = $"{FormParameters.FormContentFieldNamePrefix}{field.Guid}",
                CustomRequiredWarningText = string.Empty,
                IsRequired = field.IsRequired,
                AllowMultipleValues = field.AllowMultipleValues,
                IsNewField = true,
            };
            return contentField;
        }

        public Task<string> GetFormContentWithTransformedMetadataAsync(IEnumerable<FormContentField> contentFields, string formWithMetadata, string contentTitle, string contentDescription, string contentSubmitText)
        {
           return Task.Run(() => { return GetFormContentWithTransformedMetadata(contentFields, formWithMetadata, contentTitle, contentDescription, contentSubmitText); });
        }
        public string GetFormContentWithTransformedMetadata(IEnumerable<FormContentField> contentFields, string formWithMetadata, string contentTitle, string contentDescription, string contentSubmitText)
        {
            return formBuilder.TransformMetadataOnForm(contentFields, formWithMetadata, contentTitle, contentDescription, contentSubmitText);
        }
    }
}
