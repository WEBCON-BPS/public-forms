using System;
using System.Collections.Generic;
using System.Linq;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Service
{
    public class FormCommandService : IFormCommandService, IDisposable
    {
        private readonly IFormUnitOfWork formUnitOfWork;
        private readonly IDataEncoding dataEncoding;

        public FormCommandService(IFormUnitOfWork formUnitOfWork)
        {
            this.formUnitOfWork = formUnitOfWork?? throw new ApplicationArgumentException("Data source instance not provided");
        }
        public FormCommandService(IFormUnitOfWork formUnitOfWork, IDataEncoding dataEncoding) : this(formUnitOfWork)
        {
            this.dataEncoding = dataEncoding;
        }
        public int AddForm(DTO.Form formModel)
        {
            if (formModel == null) throw new ApplicationArgumentException("Form data model has not been provided. Could not add a new form");
            ValidateBpsObjectSelection(formModel);
            if (formModel.ContentFields == null)
                throw new ApplicationArgumentException("Could not add form because form fields have not been provided");

            var application = GetBpsObject(formUnitOfWork.BpsApplications, formModel.BpsApplicationGuid, formModel.BpsApplicationName, () => { return new BpsApplication(formModel.BpsApplicationGuid, formModel.BpsApplicationName); });
            var process = GetBpsObject(formUnitOfWork.BpsProcesses, formModel.BpsProcessGuid, formModel.BpsProcessName, () => { return new BpsProcess(formModel.BpsProcessGuid, formModel.BpsProcessName, application); });
            var workflow = GetBpsObject(formUnitOfWork.BpsWorkflows, formModel.BpsWorkflowGuid, formModel.BpsWorkflowName, () => { return new BpsWorkflow(formModel.BpsWorkflowGuid, formModel.BpsWorkflowName, process); });
            var step = GetBpsObject(formUnitOfWork.BpsWorkflowSteps, formModel.BpsWorkflowStartStepGuid, formModel.BpsWorkflowStartStepName, () => { return new BpsWorkflowStep(formModel.BpsWorkflowStartStepGuid, formModel.BpsWorkflowStartStepName, true, workflow); });
            var path = GetBpsObject(formUnitOfWork.BpsStepPaths, formModel.BpsStartStepPathGuid, formModel.BpsStartStepPathName, () => { return new BpsStepPath(formModel.BpsStartStepPathGuid, formModel.BpsStartStepPathName, step); });
            var formType = GetBpsObject(formUnitOfWork.BpsFormTypes, formModel.BpsFormTypeGuid, formModel.BpsFormTypeName, () => { return new BpsFormType(formModel.BpsFormTypeGuid, formModel.BpsFormTypeName); });

            AddFormFields(formType, formModel.ContentFields.Select(x => x.BpsFormField).ToList());

            var (clientId, clientSecret) = SetFormCustomCredentials(formModel.CustomClientId, formModel.CustomClientSecret);

            Domain.Model.Form form = new Domain.Model.Form(formModel.Name, formModel.Content, formType, path);
            if (formModel.BpsBusinessEntityGuid != Guid.Empty)
            {
                var businessEntity = GetBpsObject(formUnitOfWork.BpsBusinessEntity, formModel.BpsBusinessEntityGuid, formModel.BpsBusinessEntityName, () => { return new BpsBusinessEntity(formModel.BpsBusinessEntityGuid, formModel.BpsBusinessEntityName); });
                form.SetBusinessEntity(businessEntity.Guid);
            }
            form.SetContentTexts(formModel.ContentTitle, formModel.ContentDescription, formModel.ContentSubmitText, formModel.ContentSubmitProcessingText, formModel.ContentSubmitSuccessMessage);
            form.SetCaptchaRequired(formModel.IsCaptchaRequired);
            form.SetFrameAncestors(FrameAncestorsHelper.GetStringFromArray(formModel.FrameAncestors));
            form.SetCustomCredentials(clientId, clientSecret);
            form.SetCustomStyle(formModel.Style);
            form.SetCustomStyleLink(formModel.CustomCssLink);
            form.SetIsActive(formModel.IsActive);
            form.SetUseStandardBootstrapStyle(formModel.UseStandardBootstrapStyle);
            formUnitOfWork.Forms.Add(form);

            AddFormContentFields(form, formModel.ContentFields);

            formUnitOfWork.SaveChanges();
            return form.Id;
        }
        public void EditForm(DTO.Form formModel)
        {
            if (formModel == null) throw new ApplicationArgumentException("Form model has not been provided. Could not edit form");
            if (formModel.Id.Equals(0)) throw new ApplicationArgumentException("Form identifier has not been provided. Could not edit form.");
            ValidateBpsObjectSelection(formModel);

            var form = formUnitOfWork.Forms.FirstOrDefault(x => x.Id == formModel.Id);
            if (form == null) throw new FormNotFoundException($"Form for specified identifier {formModel.Id} not found");

            var (clientId, clientSecret) = SetFormCustomCredentials(formModel.CustomClientId, formModel.CustomClientSecret);

            var application = GetBpsObject(formUnitOfWork.BpsApplications, formModel.BpsApplicationGuid, formModel.BpsApplicationName, () => { return new BpsApplication(formModel.BpsApplicationGuid, formModel.BpsApplicationName); });
            var process = GetBpsObject(formUnitOfWork.BpsProcesses, formModel.BpsProcessGuid, formModel.BpsProcessName, () => { return new BpsProcess(formModel.BpsProcessGuid, formModel.BpsProcessName, application); });
            process?.Update(application);
            formUnitOfWork.BpsProcesses.Update(process);

            var workflow = GetBpsObject(formUnitOfWork.BpsWorkflows, formModel.BpsWorkflowGuid, formModel.BpsWorkflowName, () => { return new BpsWorkflow(formModel.BpsWorkflowGuid, formModel.BpsWorkflowName, process); });
            workflow?.Update(process);
            formUnitOfWork.BpsWorkflows.Update(workflow);

            var step = GetBpsObject(formUnitOfWork.BpsWorkflowSteps, formModel.BpsWorkflowStartStepGuid, formModel.BpsWorkflowStartStepName, () => { return new BpsWorkflowStep(formModel.BpsWorkflowStartStepGuid, formModel.BpsWorkflowStartStepName, true, workflow); });
            step?.Update(workflow);
            formUnitOfWork.BpsWorkflowSteps.Update(step);

            var path = GetBpsObject(formUnitOfWork.BpsStepPaths, formModel.BpsStartStepPathGuid, formModel.BpsStartStepPathName, () => { return new BpsStepPath(formModel.BpsStartStepPathGuid, formModel.BpsStartStepPathName, step); });
            path?.Update(step);
            formUnitOfWork.BpsStepPaths.Update(path);

            var formType = GetBpsObject(formUnitOfWork.BpsFormTypes, formModel.BpsFormTypeGuid, formModel.BpsFormTypeName, () => { return new BpsFormType(formModel.BpsFormTypeGuid, formModel.BpsFormTypeName); });
            
            if (formModel.BpsBusinessEntityGuid != form.BusinessEntityGuid)
            {
                var businessEntity = GetBpsObject(formUnitOfWork.BpsBusinessEntity, formModel.BpsBusinessEntityGuid, formModel.BpsBusinessEntityName, () => { return new BpsBusinessEntity(formModel.BpsBusinessEntityGuid, formModel.BpsBusinessEntityName); });
                form.SetBusinessEntity(businessEntity.Guid);
            }
            UpdateFormContentFields(formModel, form);
           
            form.Update(formModel.Name, formModel.Content, formType, path);
            form.SetCaptchaRequired(formModel.IsCaptchaRequired);
            form.SetFrameAncestors(FrameAncestorsHelper.GetStringFromArray(formModel.FrameAncestors));
            form.SetCustomCredentials(clientId, clientSecret);
            form.SetContentTexts(formModel.ContentTitle, formModel.ContentDescription, formModel.ContentSubmitText, formModel.ContentSubmitProcessingText, formModel.ContentSubmitSuccessMessage);
            form.SetCustomStyle(formModel.Style);
            form.SetCustomStyleLink(formModel.CustomCssLink);
            form.SetIsActive(formModel.IsActive);
            form.SetUseStandardBootstrapStyle(formModel.UseStandardBootstrapStyle);
            formUnitOfWork.Forms.Update(form);
            formUnitOfWork.SaveChanges();
        }
        public void RemoveForm(int formId)
        {
            if (formId == 0) throw new ApplicationArgumentException("Form to delete not provided");

            var form = formUnitOfWork.Forms.FirstOrDefault(x => x.Id == formId);
            if (form == null) return;
            formUnitOfWork.Forms.Delete(form);
            formUnitOfWork.SaveChanges();
        }
        private void UpdateFormContentFields(DTO.Form formModel, Domain.Model.Form form)
        {
            if (formModel.ContentFields != null)
            {
                var formContentFieldsGuids = formUnitOfWork.FormContentFields.GetFiltered(x => x.Form.Guid == form.Guid).ToList();

                var newContentFields = formModel.ContentFields.Where(x => x.BpsFormField != null && x.IsNewField == true).ToList();
                var removedContentFields = formContentFieldsGuids.Where(x => !formModel.ContentFields.Any(z => z.Guid == x.Guid)).ToList();
                var existingContentFields = formModel.ContentFields.Where(x => formContentFieldsGuids.Any(z => z.Guid == x.Guid)).ToList();

                if (newContentFields?.Any() ?? false)
                {
                    AddFormFields(form.BpsFormType, newContentFields.Select(x => x.BpsFormField).ToList());
                    foreach (var newField in newContentFields)
                        AddFormContentField(form, newField);
                }
                if (existingContentFields?.Any() ?? false)
                    foreach (var existingField in existingContentFields)
                        UpdateFormContentField(existingField);

                if (removedContentFields?.Any() ?? false)
                    foreach (var removed in removedContentFields)
                        RemoveContentFields(removed);
            }
        }
        private void RemoveContentFields(FormContentField removed)
        {
            formUnitOfWork.FormContentFields.Delete(removed);
        }
        private void ValidateBpsObjectSelection(DTO.Form formModel)
        {
            if (formModel.BpsApplicationGuid == Guid.Empty)
                throw new ApplicationArgumentException("Could not add form because application has not been selected");
            if (formModel.BpsFormTypeGuid == Guid.Empty)
                throw new ApplicationArgumentException("Could not add form because formType has not been selected");
            if (formModel.BpsWorkflowStartStepGuid == Guid.Empty)
                throw new ApplicationArgumentException("Could not add form because start step has not been selected");
            if (formModel.BpsStartStepPathGuid == Guid.Empty)
                throw new ApplicationArgumentException("Could not add form because step path has not been selected");
            if (formModel.BpsProcessGuid == Guid.Empty)
                throw new ApplicationArgumentException("Could not add form because process has not been selected");
            if (formModel.BpsWorkflowGuid == Guid.Empty)
                throw new ApplicationArgumentException("Could not add form because workflow has not been selected");
        }
        private (string clientId, string clientSecret) SetFormCustomCredentials(string clientId, string clientSecret)
        {
            return (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret)) ? (clientId, dataEncoding.Encode(clientSecret)) : (string.Empty, string.Empty);
        }
        private void AddFormFields(Domain.Model.BpsFormType formType, List<DTO.FormField> fields)
        {
            var formFields = formUnitOfWork.BpsFormFields.GetFiltered(x => x.BPSFormType.Guid == formType.Guid).ToList();
            foreach (DTO.FormField field in fields)
            {
                if (field == null) continue;
                var currentField = formFields.FirstOrDefault(x => x.Guid.Equals(field.Guid));
                if (currentField != null)
                {
                    if (field.Type != currentField.Type || field.Name != currentField.Name)
                    {
                        currentField.Update(field.Type, field.Name);
                        formUnitOfWork.BpsFormFields.Update(currentField);
                    }
                    continue;
                }
                formUnitOfWork.BpsFormFields.Add(new Domain.Model.BpsFormField(field.Guid, field.Name, field.Type, formType)
                {
                    IsReadonly = field.IsReadonly,
                    IsRequired = field.IsRequired
                });
            }
        }
        private void UpdateFormContentField(DTO.FormContentField contentFieldModel)
        {
            var contentField = formUnitOfWork.FormContentFields.FirstOrDefault(x => x.Guid == contentFieldModel.Guid);
            if (contentField != null)
            {
                contentField.SetCustomWarningText(contentFieldModel.CustomRequiredWarningText);
                contentField.SetCustomName(contentFieldModel.CustomName);
                contentField.SetIsRequired(contentFieldModel.IsRequired);
                contentField.SetAllowMultipleValue(contentFieldModel.AllowMultipleValues);
                formUnitOfWork.FormContentFields.Update(contentField);
            }
        }
        private void AddFormContentFields(Form form, List<DTO.FormContentField> contentFieldModelList)
        {
            foreach (DTO.FormContentField contentFieldModel in contentFieldModelList)
            {
                AddFormContentField(form, contentFieldModel);
            }
        }
        private void AddFormContentField(Form form, DTO.FormContentField contentFieldModel)
        {
            BpsFormField field = null;

            field = (contentFieldModel.BpsFormField != null) ? formUnitOfWork.BpsFormFields.FirstOrDefault(x => x.Guid == contentFieldModel.BpsFormField.Guid) : null;
            var contentField = new Domain.Model.FormContentField(form, field, contentFieldModel.Name, contentFieldModel.CustomName);
            contentField.SetIsRequired(contentFieldModel.IsRequired);
            contentField.SetCustomWarningText(contentFieldModel.CustomRequiredWarningText);
            contentField.SetAllowMultipleValue(contentFieldModel.AllowMultipleValues);
            formUnitOfWork.FormContentFields.Add(contentField);
        }
        private T GetBpsObject<T>(IRepository<T> repo, Guid objectGuid, string objectName, Func<T> createNewInstance)
            where T : BpsEntity, new()
        {
            var entity = repo.FirstOrDefault(x => x.Guid == objectGuid);

            if (entity == null)
            {
                entity = createNewInstance.Invoke();
                repo.Add(entity);
            }
            else if (entity.Name != objectName)
            {
                entity.SetName(objectName);
                repo.Update(entity);
            }

            return entity;
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                GC.SuppressFinalize(this);
                formUnitOfWork.Dispose();
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
