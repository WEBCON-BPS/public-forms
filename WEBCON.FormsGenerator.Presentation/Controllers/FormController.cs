using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Presentation.Controllers
{
    /// <summary>
    /// Application form controller
    /// </summary>
    public class FormController : Controller
    {
        private readonly IFormCommandService formCommandService;
        private readonly IFormQueryService formQueryService;
        private readonly ILogger<FormController> logger;
        private readonly IStringLocalizer<FormController> localizer;
        private readonly IMapper mapper;

        public FormController(IFormCommandService formCommandService, IFormQueryService formQueryService, ILogger<FormController> logger, IStringLocalizer<FormController> localizer, IMapper mapper)
        {          
            this.formCommandService = formCommandService;
            this.formQueryService = formQueryService;
            this.logger = logger;
            this.localizer = localizer;
            this.mapper = mapper;
        }     
        [Authorize]
        public IActionResult Create()
        {
            FormViewModel formViewModel = new FormViewModel
            {
                MessageParameters = GetMessageParameters(),
                ContentSubmitProcessingText = "Sending...",
                ContentSubmitText = "Submit",
                ContentSubmitSuccessMessage = $"Successfully added element with signature {FormParameters.Signature}",
                IsActive = true,
                UseStandardBootstrapStyle = true
            };
            return View(formViewModel);
        }

        private Dictionary<string, string> GetMessageParameters()
        {
            return new Dictionary<string, string>
            {
                { "Signature", FormParameters.Signature },
                { "Element ID", FormParameters.ElementId }
            };
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(FormViewModel formViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(formViewModel);

                if (formViewModel.BpsApplicationGuid == Guid.Empty)
                {
                    ModelState.AddModelError(nameof(formViewModel.BpsApplicationGuid), localizer["Application is required"]);
                    return View(formViewModel);
                }
                if (formViewModel.BpsProcessGuid == Guid.Empty)
                {
                    ModelState.AddModelError(nameof(formViewModel.BpsProcessGuid), localizer["Process is required"]);
                    return View(formViewModel);
                }
                if (formViewModel.BpsWorkflowGuid == Guid.Empty)
                {
                    ModelState.AddModelError(nameof(formViewModel.BpsWorkflowGuid), localizer["Workflow is required"]);
                    return View(formViewModel);
                }
                if (formViewModel.BpsFormTypeGuid == Guid.Empty)
                {
                    ModelState.AddModelError(nameof(formViewModel.BpsFormTypeGuid), localizer["Form type is required"]);
                    return View(formViewModel);
                }
                if (!SetCustomConnection(formViewModel))
                    return View(formViewModel);
              
                Form form = mapper.Map<FormViewModel, Form>(formViewModel);
                if (form != null)
                {
                    form.ContentFields = formViewModel.ContentFields?.Select(x => new WEBCON.FormsGenerator.BusinessLogic.Application.DTO.FormContentField
                    {
                        Name = x.Name,
                        CustomName = x.CustomName,
                        CustomRequiredWarningText = x.CustomRequiredWarningText,
                        IsRequired = x.IsRequired,
                        AllowMultipleValues = x.AllowMultipleValues,
                        BpsFormField = new FormField
                        {
                            Guid = x.BpsFormFieldGuid,
                            Type = x.Type,
                            IsReadonly = x.BpsIsReadonly,
                            IsRequired = x.BpsIsRequired,
                            Name = x.BpsName
                        }
                    }).ToList();
                }
                int formId = formCommandService.AddForm(form);
                if (formId.Equals(0))
                {
                    ModelState.AddModelError("", localizer["The form has not been created. Please, contact with administrator."]);
                    return View(formViewModel);
                }
                logger?.LogInformation($"Added form {formId}");
                return RedirectToAction("Edit", new { id = formId });
            }
            catch (BusinessLogic.Domain.Exceptions.ApplicationException ex)
            {
                ModelState.AddModelError("", localizer[ex.Message]);
                logger?.LogError(ex.Message);
                return View(formViewModel);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.Message);
                throw;
            }
        }
        [Authorize]
        public IActionResult Edit(int id)
        {
            var form = formQueryService.GetForm(id);
            string formUrl = @$"{(HttpContext.Request.IsHttps ? "https" : "http")}://{HttpContext.Request.Host}/BPSForm/{form.Guid}";
            var viewModel = mapper.Map<Form, FormViewModel>(form);
            if (viewModel != null)
            {
                viewModel.MessageParameters = GetMessageParameters();
                viewModel.CustomConnectionSelected = !string.IsNullOrEmpty(form.CustomClientId) && !string.IsNullOrEmpty(form.CustomClientSecret);
                viewModel.IFrame = $@"<iframe src=""{formUrl}"" title=""WEBCON Forms Generator IFrame"" width=""400"" height=""300""></iframe>";
                viewModel.FrameUrl = formUrl;
            }
            return View(viewModel);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Edit(FormViewModel formViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(formViewModel);

                if (!SetCustomConnection(formViewModel))
                    return View(formViewModel);
  
                Form form = mapper.Map<FormViewModel, Form>(formViewModel);
                if (form != null)
                {
                    form.ContentFields = formViewModel.ContentFields?.Select(x => new WEBCON.FormsGenerator.BusinessLogic.Application.DTO.FormContentField
                    {
                        Name = x.Name,
                        CustomName = x.CustomName,
                        Guid =x.Guid,
                        CustomRequiredWarningText = x.CustomRequiredWarningText,
                        IsRequired = x.IsRequired,
                        IsNewField = x.IsNewField,
                        AllowMultipleValues = x.AllowMultipleValues,
                        BpsFormField = new FormField
                        {
                            Type = x.Type,
                            Guid = x.BpsFormFieldGuid,
                            IsReadonly = x.BpsIsReadonly,
                            IsRequired = x.BpsIsRequired,
                            Name = x.BpsName
                        }
                    }).ToList();
                }
                formCommandService.EditForm(form);
                logger?.LogInformation($"Edited form {formViewModel.Id}");
                return RedirectToAction("Index", "Forms");
            }
            catch (BusinessLogic.Domain.Exceptions.ApplicationException ex)
            {
                ModelState.AddModelError("", localizer[ex.Message]);
                logger?.LogError(ex.Message);
                return View(formViewModel);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.Message);
                throw;
            }
        }
        private bool SetCustomConnection(FormViewModel formViewModel)
        {
            if (formViewModel.CustomConnectionSelected && (string.IsNullOrEmpty(formViewModel.CustomClientId) || string.IsNullOrEmpty(formViewModel.CustomClientSecret)))
            {
                ModelState.AddModelError("", localizer["API url was not specified"]);
                ModelState.AddModelError(nameof(formViewModel.CustomClientId), localizer["Custom connection client id should be passed"]);
                ModelState.AddModelError(nameof(formViewModel.CustomClientSecret), localizer["Custom connection client secret should be passed"]);
                return false;
            }
            if (!formViewModel.CustomConnectionSelected && (!string.IsNullOrEmpty(formViewModel.CustomClientId) || !string.IsNullOrEmpty(formViewModel.CustomClientSecret)))
            {
                formViewModel.CustomClientId = "";
                formViewModel.CustomClientSecret = "";          
            }
            return true;
        }
        public IActionResult Delete(int id)
        {
            return View(new FormViewModel { Id = id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            formCommandService.RemoveForm(id);
            logger?.LogInformation($"Deleted form {id}");
            return RedirectToAction("Index", "Forms");
        }        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode > 0)
            {
                if (statusCode == 404)
                    return View(new ErrorViewModel { StatusCode = statusCode, Message = localizer["Page not found"] });
                else
                    return View(new ErrorViewModel { StatusCode = statusCode, Message = localizer["Something went wrong. Please contact with administrator"] });
            }
            var code = 500;
            string errorMessage = localizer["Internal Server Error. Contact with administrator"];
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (context != null)
            {
                var exception = context.Error;
         
                if (exception != null)
                {
                    logger?.LogError(exception, exception.Message);
                    if (exception is FormNotFoundException)
                    {
                        code = 404;
                        errorMessage = localizer["Form not found"];
                    }
                    else if (exception is UnauthorizedAccessException)
                    {
                        code = 401;
                        errorMessage = localizer["Unauthorized"];
                    }
                    else if(exception is BusinessLogic.Domain.Exceptions.ApplicationException)
                    {
                        errorMessage = exception.Message;
                    }
                }
            }
            return View(new ErrorViewModel { StatusCode = code, Message = errorMessage });
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), HttpOnly = true }
            );
            return LocalRedirect(returnUrl);
        }
    }
}
