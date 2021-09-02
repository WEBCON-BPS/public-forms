using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Presentation.Controllers
{
    public class FormContentController : Controller
    {
        private readonly IFormContentService formContentService;
        private readonly IBpsQueryService bpsFormService;
        private readonly ILogger<FormContentController> logger;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<FormContentController> localizer;

        public FormContentController(IFormContentService formContentService, IBpsQueryService bpsFormService, ILogger<FormContentController> logger, IMapper mapper, IStringLocalizer<FormContentController> localizer)
        {
            this.formContentService = formContentService;
            this.bpsFormService = bpsFormService;
            this.logger = logger;
            this.mapper = mapper;
            this.localizer = localizer;
        }      
        [HttpPost]
        public async Task<JsonResult> Create(Guid formTypeGuid, string contentTitle, Guid workflowGuid, Guid stepGuid, string token = "")
        {
            try
            {
                var result = await bpsFormService.GetFormFieldsAsync(formTypeGuid, workflowGuid, stepGuid, token);
                var content = formContentService.CreateFormContent(result, contentTitle);
                if (content == null)
                    throw new BusinessLogic.Domain.Exceptions.ApplicationException("Returned form content was empty");
                var mappedResult = mapper.Map<FormContent, FormContentViewModel>(content);
                return Json(mappedResult);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, ex.Message);
                return Json(localizer?[ex.Message] ?? ex.Message);
            }
        }
        [Authorize]
        public async Task<JsonResult> GetWithTransformedMeta(List<FormContentFieldViewModel> contentFields, string contentWithMetadata, string contentTitle, string contentDescription, string contentSubmitText)
        {
            try
            {
                List<FormContentField> fields = mapper.Map<List<FormContentFieldViewModel>, List<FormContentField>>(contentFields);
                string formContentTransformed = await formContentService.GetFormContentWithTransformedMetadataAsync(fields, contentWithMetadata, contentTitle, contentDescription, contentSubmitText);
                return Json(new { formContentTransformed });
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, ex.Message);
                return Json(localizer?[ex.Message] ?? ex.Message);
            }
        }
    }
}
