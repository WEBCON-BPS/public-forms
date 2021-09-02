using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;

namespace WEBCON.FormsGenerator.Presentation.Controllers
{
    public class FormContentRefreshController : Controller
    {
        private readonly IFormContentRefreshService refreshFormContentService;
        private readonly IBpsQueryService bpsFormService;
        private readonly ILogger<FormContentRefreshController> logger;
        private readonly IStringLocalizer<FormContentRefreshController> localizer;

        public FormContentRefreshController(IFormContentRefreshService refreshFormContentService, IBpsQueryService bpsFormService, ILogger<FormContentRefreshController> logger, IStringLocalizer<FormContentRefreshController> localizer)
        {
            this.refreshFormContentService = refreshFormContentService;
            this.bpsFormService = bpsFormService;
            this.logger = logger;
            this.localizer = localizer;
        }
        [HttpPost]
        public async Task<JsonResult> RefreshContent(Guid bpsFormTypeGuid, Guid bpsWorkflowGuid, Guid formGuid, Guid stepGuid, string token = "")
        {
            try
            {
                logger?.LogInformation($"Refresh form with guid {formGuid}");
                var fields = await bpsFormService.GetFormFieldsAsync(bpsFormTypeGuid, bpsWorkflowGuid, stepGuid, token);
                var result = await refreshFormContentService.RefreshFormContentAsync(fields, formGuid);
                if (result != null)
                {
                    return Json(new ViewModels.FormContentViewModel
                    {
                        FormContentTransformed = result.FormContentTransformed,
                        FormContentWithMetadata = result.FormContentWithMetadata,
                        ContentFields = result.ContentFields?.Select(x => new ViewModels.FormContentFieldViewModel
                        {
                            CustomName = x.CustomName,
                            IsRequired = x.IsRequired,
                            Name = x.Name,
                            AllowMultipleValues = x.AllowMultipleValues,
                            Type = (x.BpsFormField == null) ? BusinessLogic.Domain.Enum.FormFieldType.Undefined : x.BpsFormField.Type,
                            BpsIsReadonly = x.BpsFormField != null && x.BpsFormField.IsReadonly,
                            BpsIsRequired = x.BpsFormField != null && x.BpsFormField.IsRequired,
                            Guid = x.Guid,
                            CustomRequiredWarningText = x.CustomRequiredWarningText,
                            IsNewField = x.IsNewField,
                            BpsName = (x.BpsFormField == null) ? string.Empty : x.BpsFormField.Name,
                            BpsFormFieldGuid = (x.BpsFormField == null) ? Guid.Empty : x.BpsFormField.Guid
                        }).ToList()
                    });
                }
                return Json("");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, ex.Message);
                return Json(localizer?[ex.Message] ?? ex.Message);
            }
        }
    }
}
