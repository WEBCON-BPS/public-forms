using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Service
{
    public class FormContentRefreshService : IFormContentRefreshService
    {
        private readonly IFormUnitOfWork formUnitOfWork;
        private readonly IFormContentService formContentService;
        private readonly IFormBuilder formBuilder;

        public FormContentRefreshService(IFormUnitOfWork formUnitOfWork, IFormContentService formContentService, IFormBuilder formBuilder)
        {
            this.formUnitOfWork = formUnitOfWork;
            this.formContentService = formContentService;
            this.formBuilder = formBuilder;
        }
        public Task<FormContent> RefreshFormContentAsync(IEnumerable<FormField> formFields, Guid formGuid)
        {
            return Task.Run(() => RefreshFormContent(formFields, formGuid));
        }
        public FormContent RefreshFormContent(IEnumerable<FormField> formFields, Guid formGuid)
        {
            if (formFields == null) return null;
            var form = formUnitOfWork.Forms.FirstOrDefault(x => x.Guid == formGuid);
            if (form == null)
                throw new ApplicationArgumentException($"Form with guid {formGuid} not found");

            var contentFields = formUnitOfWork.FormContentFields.GetFiltered(x => x.Form.Guid == form.Guid);
            List<FormField> newFields = new List<FormField>();
            List<Domain.Model.FormContentField> removedFields = new List<Domain.Model.FormContentField>();
            string body = form.Content;
            if (contentFields == null)
            {
                newFields.AddRange(formFields);
            }
            else
            {          
                foreach (FormField formField in formFields)
                {
                    var existingContentField = contentFields.FirstOrDefault(x => x.BpsFormField.Guid == formField.Guid);
                    if (existingContentField == null) newFields.Add(formField);
                    else
                    {
                        if (formField is IChoice choiceableField)
                           body = formBuilder.UpdateChoices(formField, body);                        
                    }
                }
                foreach (var contentField in contentFields)
                {
                    if (!formFields.Any(x => x.Guid == contentField.BpsFormField.Guid))
                       body = formBuilder.RemoveFieldFromExistingForm(contentField.BpsFormField.Guid, body);
                }
            }  
            if (newFields.Any())
                body = AddNewFields(newFields, body);
    
            var fields = formFields.Select(x => new DTO.FormContentField
            {
                BpsFormField = x,
                CustomName = x.Name,               
                Guid = contentFields?.ToList().FirstOrDefault(c=>c.BpsFormField.Guid == x.Guid)?.Guid ?? Guid.Empty,
                Name = $"{FormParameters.FormContentFieldNamePrefix}{x.Guid}",
                CustomRequiredWarningText = string.Empty,
                IsRequired = x.IsRequired,
                AllowMultipleValues = x.AllowMultipleValues,
                IsNewField = newFields?.Any(field => field.Guid == x.Guid) ?? false
            });
            var bodyTransformed = formContentService.GetFormContentWithTransformedMetadata(fields, body, form.ContentTitle, form.ContentDescription, form.ContentSubmitText);

            return new FormContent
            {
                ContentFields = fields.ToList(),
                FormContentTransformed = bodyTransformed,
                FormContentWithMetadata = body
            };
        }
        private string AddNewFields(List<FormField> newFields, string formContent)
        {
            List<string> newContentFields = new List<string>();
            foreach (var newField in newFields)
            {
                string field = formContentService.CreateFormContentInput(newField);
                if (string.IsNullOrEmpty(field)) continue;
                newContentFields.Add(field);        
            }
            return formBuilder.EmbedInputsInExistingForm(newContentFields, formContent);
        }
    }
}
