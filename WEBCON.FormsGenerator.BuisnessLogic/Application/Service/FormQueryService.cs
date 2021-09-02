using System;
using System.Linq;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Service
{
    public class FormQueryService : IFormQueryService, IDisposable
    {
        private readonly IFormUnitOfWork formUnitOfWork;
        private readonly IFormBuilder formBuilder;
        private readonly IDataEncoding dataEncoding;

        public FormQueryService(IFormUnitOfWork formUnitOfWork, IFormBuilder formBuilder, IDataEncoding dataEncoding)
        {
            this.formUnitOfWork = formUnitOfWork ?? throw new ApplicationArgumentException("Data source instance not provided");
            this.formBuilder = formBuilder ?? throw new ApplicationArgumentException("Form builder instance not provided");
            this.dataEncoding = dataEncoding;
        }
        public Task<IPagedList<Domain.Model.Form>> GetForms(string searchString, int pageNumber, string sortOption, int itemsOnPage)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return formUnitOfWork.Forms.GetFilteredPaginated(s => s.Name.Contains(searchString)
                || s.BpsFormType.Name.Contains(searchString)
                || s.BpsStepPath.BpsWorkflowStep.BpsWorkflow.BpsProcess.Name.Contains(searchString) || s.BpsStepPath.BpsWorkflowStep.BpsWorkflow.Name.Contains(searchString), pageNumber, itemsOnPage, sortOption);
            }
            return formUnitOfWork.Forms.GetFilteredPaginated(x => 1 == 1, pageNumber, itemsOnPage, sortOption);
        }
        public DTO.Form GetForm(int id)
        {
            if (id.Equals(0)) throw new ApplicationArgumentException("Form identifier has not been provided. Could not get form.");
            Domain.Model.Form form = formUnitOfWork.Forms.FirstOrDefault(x => x.Id == id);
            if (form == null) throw new FormNotFoundException($"Form for specified identifier {id} not found");
            return GetForm(form);
        }
        public DTO.Form GetForm(Guid guid)
        {
            if (guid.Equals(Guid.Empty)) throw new ApplicationArgumentException("Form identifier has not been provided. Could not get form.");
            Domain.Model.Form form = formUnitOfWork.Forms.FirstOrDefault(x => x.Guid == guid);
            if (form == null) throw new FormNotFoundException($"Form for specified identifier not found");
            return GetForm(form);
        }
        private DTO.Form GetForm(Domain.Model.Form form)
        {
            DTO.Form dtoForm = new DTO.Form
            {
                Id = form.Id,
                Guid = form.Guid,
                Content = form.Content,
                Style = form.Style,
                IsCaptchaRequired = form.IsCaptchaRequired ?? false,
                Name = form.Name,
                IsActive = form.IsActive,
                ContentFields = formUnitOfWork.FormContentFields.GetFiltered(x => x.Form.Guid == form.Guid)?.Select(x => new FormContentField
                {
                    CustomName = x.CustomName,
                    Name = x.Name,
                    BpsFormField = (x.BpsFormField != null) ? new FormField
                    {
                        IsRequired = x.BpsFormField.IsRequired,
                        IsReadonly = x.BpsFormField.IsReadonly,
                        Type=x.BpsFormField.Type,
                        Guid = x.BpsFormField.Guid,
                        Name = x.BpsFormField.Name,                       
                    } : null,                   
                    Guid = x.Guid,
                    IsRequired = x.IsRequired,
                    AllowMultipleValues = x.AllowMultipleValue,
                    CustomRequiredWarningText = x.CustomRequiredWarningText
                }).ToList(),
                BpsFormTypeGuid = form.BpsFormType?.Guid ?? Guid.Empty,
                BpsFormTypeName = form.BpsFormType?.Name,
                BpsWorkflowGuid = form.BpsStepPath?.BpsWorkflowStep?.BpsWorkflow?.Guid ?? Guid.Empty,
                BpsWorkflowName = form.BpsStepPath?.BpsWorkflowStep?.BpsWorkflow?.Name,
                BpsWorkflowStartStepGuid = form.BpsStepPath?.BpsWorkflowStep?.Guid ?? Guid.Empty,
                BpsWorkflowStartStepName = form.BpsStepPath?.BpsWorkflowStep?.Name,
                BpsProcessGuid = form.BpsStepPath?.BpsWorkflowStep?.BpsWorkflow?.BpsProcess?.Guid ?? Guid.Empty,
                BpsProcessName = form.BpsStepPath?.BpsWorkflowStep?.BpsWorkflow?.BpsProcess?.Name,
                BpsApplicationGuid = form.BpsStepPath?.BpsWorkflowStep?.BpsWorkflow?.BpsProcess?.BpsApplication?.Guid ?? Guid.Empty,
                BpsApplicationName = form.BpsStepPath?.BpsWorkflowStep?.BpsWorkflow?.BpsProcess?.BpsApplication?.Name,
                BpsStartStepPathGuid = form.BpsStepPath?.Guid ?? Guid.Empty,
                BpsStartStepPathName = form.BpsStepPath?.Name,
                BpsBusinessEntityName = formUnitOfWork.BpsBusinessEntity.FirstOrDefault(x=>x.Guid == form.BusinessEntityGuid)?.Name,               
                CustomClientId = form.ClientId,
                CustomCssLink = form.CustomCssLink,
                BpsBusinessEntityGuid = form.BusinessEntityGuid, 
                ContentSubmitSuccessMessage = form.ContentSubmitSuccessMessage,
                ContentSubmitProcessingText = form.ContentSubmitProcessingText,
                ContentDescription = form.ContentDescription,
                ContentSubmitText = form.ContentSubmitText,
                ContentTitle = form.ContentTitle,
                CustomClientSecret = form.ClientSecret,
                UseStandardBootstrapStyle = form.UseStandardBootstrapStyle,
                FrameAncestors = FrameAncestorsHelper.GetArrayFromString(form.FrameAncestors),
            };
            dtoForm.ContentTransformed = formBuilder.TransformMetadataOnForm(dtoForm.ContentFields, dtoForm.Content, dtoForm.ContentTitle, dtoForm.ContentDescription, dtoForm.ContentSubmitText);
            try
            {
                if (!string.IsNullOrEmpty(form.ClientSecret))
                    dtoForm.CustomClientSecret = dataEncoding?.Decode(form.ClientSecret);
            }
            catch
            {
                dtoForm.CustomClientSecret = dtoForm.CustomClientId = string.Empty;
            }
            return dtoForm;
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (!this.disposed && disposing)
                {
                    GC.SuppressFinalize(this);
                    formUnitOfWork.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
