using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WEBCON.FormsGenerator.Presentation.Configuration;
using WEBCON.FormsGenerator.Presentation.Configuration.Model;

namespace WEBCON.FormsGenerator.Presentation.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IWritableOptions<Logging> loggingConfiguration;
        private readonly IStringLocalizer<ConfigurationController> localizer;
        private readonly IReadOnlyConfiguration config;

        private bool IsHostedOnAzure => !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
        public ConfigurationController(IWritableOptions<Logging> loggingConfiguration, IStringLocalizer<ConfigurationController> localizer, IReadOnlyConfiguration config)
        {
            this.loggingConfiguration = loggingConfiguration;
            this.localizer = localizer;
            this.config = config;
        }
        [Authorize]
        public IActionResult Index()
        {
            ViewModels.ConfigurationViewModel model = new ViewModels.ConfigurationViewModel()
            {
                ApiUrl = config.ApiSettings.Url,
                ClientId = config.ApiSettings.ClientId,
                DatabaseId = config.ApiSettings.DatabaseId,
                CaptchaApiKey = config.CaptchaSettings.APIKey ?? string.Empty,
                ClientSecret = config.ApiSettings.ClientSecret,
                LoggingLevel = loggingConfiguration?.Value.LogLevel.Default,
                IsHostedOnAzure = IsHostedOnAzure
            };

            return View(model);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Index(ViewModels.ConfigurationViewModel model)
        {
            if(loggingConfiguration!=null && model.LoggingLevel!=null)
            {
                loggingConfiguration.Update(x => x.LogLevel.Default = model.LoggingLevel);
            }
            model.IsHostedOnAzure = IsHostedOnAzure;
            return Saved(model);
        }
        [Authorize]
        public IActionResult Saved(ViewModels.ConfigurationViewModel model)
        {
            ViewBag.Info = localizer["Data saved correctly"];
            return View("Index", model);
        }


    }
}
