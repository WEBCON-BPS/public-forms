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
        private readonly IWritableOptions<ApiSettings> apiConfiguration;
        private readonly IWritableOptions<CaptchaSettings> captchaConfiguration;
        private readonly IWritableOptions<Logging> loggingConfiguration;
        private readonly IStringLocalizer<ConfigurationController> localizer;

        private bool IsHostedOnAzure => !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
        public ConfigurationController(IWritableOptions<ApiSettings> apiConfiguration, IWritableOptions<CaptchaSettings> captchaConfiguration, IWritableOptions<Logging> loggingConfiguration, IStringLocalizer<ConfigurationController> localizer)
        {
            this.apiConfiguration = apiConfiguration;
            this.captchaConfiguration = captchaConfiguration;
            this.loggingConfiguration = loggingConfiguration;
            this.localizer = localizer;
        }
        [Authorize]
        public IActionResult Index()
        {
            ViewModels.ConfigurationViewModel model = new ViewModels.ConfigurationViewModel()
            {
                ApiUrl = apiConfiguration?.Value.Url,
                ClientId = apiConfiguration?.Value.ClientId,
                DatabaseId = apiConfiguration?.Value.DatabaseId ?? 0,
                CaptchaApiKey = captchaConfiguration?.Value.APIKey ?? string.Empty,
                ClientSecret = apiConfiguration?.Value.ClientSecret,
                LoggingLevel = loggingConfiguration?.Value.LogLevel.Default,
                IsHostedOnAzure = IsHostedOnAzure
            };

            return View(model);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Index(ViewModels.ConfigurationViewModel model)
        {
            if (apiConfiguration != null)
            {
                if (string.IsNullOrEmpty(apiConfiguration.Value.AppKey))
                    apiConfiguration.Update(x => x.AppKey = Guid.NewGuid().ToString());
                apiConfiguration.Update(x => x.Url = model.ApiUrl);
                if (!IsHostedOnAzure)
                {
                    apiConfiguration.Update(x => x.ClientId = model.ClientId);
                    apiConfiguration.Update(x => x.ClientSecret = model.ClientSecret);
                }
                apiConfiguration.Update(x => x.DatabaseId = model.DatabaseId);
            }
            if(captchaConfiguration !=null)
            {
                captchaConfiguration.Update(x => x.APIKey = model.CaptchaApiKey);
            }
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
