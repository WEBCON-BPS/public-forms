using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.Presentation.Configuration.Model;

namespace WEBCON.FormsGenerator.Presentation.Configuration
{
    public static class ReadOnlyConfigurationExtension
    {
        public static void AddReadOnlyConfiguration(this IServiceCollection services, out ApiSettings apiConfig)
        {
            var apiSettings = new ApiSettings()
            {
                AppKey = Environment.GetEnvironmentVariable("APP_BPS_PORTAL_APP_KEY"),
                ClientId = Environment.GetEnvironmentVariable("APP_BPS_PORTAL_CLIENT_ID"),
                ClientSecret = Environment.GetEnvironmentVariable("APP_BPS_PORTAL_CLIENT_SECRET"),
                DatabaseId = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APP_BPS_PORTAL_DB_ID")) ? 1 : Int32.Parse(Environment.GetEnvironmentVariable("APP_BPS_PORTAL_DB_ID")),
                Url = Environment.GetEnvironmentVariable("APP_BPS_PORTAL_URL")
            };
            var captchaSettings = new CaptchaSettings()
            {
                APIKey = Environment.GetEnvironmentVariable("APP_RECAPTCHA_API_KEY")
            };
            var additionalSettings = new AdditionalSettings()
            {
                BasePath = Environment.GetEnvironmentVariable("APP_BASE_DOMAIN"),
                Login = Environment.GetEnvironmentVariable("APP_ADMIN_LOGIN"),
                Password = Environment.GetEnvironmentVariable("APP_ADMIN_PASSWORD")
            };
            services.AddScoped<IReadOnlyConfiguration, ReadOnlyConfiguration>(x => new ReadOnlyConfiguration(apiSettings, captchaSettings, additionalSettings));
            apiConfig = apiSettings;
        }
    }
}
