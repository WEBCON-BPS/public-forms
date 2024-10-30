using System;
using Microsoft.AspNetCore.Mvc;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.Presentation.Configuration.Model;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Presentation.Controllers
{
    public class FormContentPreviewController : Controller
    {
        private readonly IFormQueryService formQueryService;
        private readonly IReadOnlyConfiguration configuration;

        public FormContentPreviewController(IReadOnlyConfiguration configuration, IFormQueryService formQueryService)
        {
            this.configuration = configuration;
            this.formQueryService = formQueryService;
        }
        [Route("BPSForm/{formGuid}")]
        public IActionResult Index(string formGuid)
        {
            if (!Guid.TryParse(formGuid, out Guid guid))
                throw new ApplicationArgumentException("Form guid has not been provided");
            Form form = formQueryService.GetForm(guid);

            ViewData["Error"] = TempData["Error"];
            if (form.FrameAncestors?.Length > 0)
                HttpContext.Response.Headers.Add("Content-Security-Policy", @$"frame-ancestors 'self' {string.Join(" ", form.FrameAncestors)}");
            else
                HttpContext.Response.Headers.Add("Content-Security-Policy", @$"frame-ancestors *");

            return View(new FormContentPreviewViewModel
            {
                Html = form.ContentTransformed,
                CustomCss = form.Style,
                CustomCssLink = form.CustomCssLink,
                IsCaptchaRequired = form.IsCaptchaRequired,
                UseStandardBootstrapStyle = form.UseStandardBootstrapStyle,
                CaptchaKey = configuration.CaptchaSettings.APIKey,
                FormGuid = form.Guid,
                ContentSubmitProcessingText = form.ContentSubmitProcessingText,
                ContentSubmitSuccessMessage = string.IsNullOrEmpty(form.ContentSubmitSuccessMessage) ? $"Successfully added element with signature {FormParameters.Signature}" : form.ContentSubmitSuccessMessage,
                IsReadonly = false,        
                IsActive = form.IsActive
            });
        }
        
        public IActionResult Preview(string formContent, bool useStandardBootstrapStyle, string customCssLink, string customCss)
        {
            return View("Index", new FormContentPreviewViewModel
            {
                Html = formContent,
                UseStandardBootstrapStyle = useStandardBootstrapStyle,
                CustomCssLink = customCssLink,
                FormGuid = Guid.Empty,
                CaptchaKey = "",
                IsCaptchaRequired = false,
                CustomCss = customCss,
                IsReadonly = true,
                IsActive = true
            });
        }
    }
}
