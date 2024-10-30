using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBCON.FormsGenerator.Presentation.Configuration.Model
{
    public class ReadOnlyConfiguration : IReadOnlyConfiguration
    {
        public ApiSettings ApiSettings { get; }
        public CaptchaSettings CaptchaSettings { get; }
        public AdditionalSettings AdditionalSettings { get; }

        public ReadOnlyConfiguration(ApiSettings apiSettings, CaptchaSettings captchaSettings, AdditionalSettings additionalSettings)
        {
            ApiSettings = apiSettings;
            CaptchaSettings = captchaSettings;
            AdditionalSettings = additionalSettings;
        }
    }
}
