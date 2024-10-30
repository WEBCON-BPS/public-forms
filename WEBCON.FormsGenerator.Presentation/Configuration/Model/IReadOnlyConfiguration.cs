using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBCON.FormsGenerator.Presentation.Configuration.Model
{
    public interface IReadOnlyConfiguration
    {
        public ApiSettings ApiSettings { get; }
        public CaptchaSettings CaptchaSettings { get;}
        public AdditionalSettings AdditionalSettings { get;}
    }
}
