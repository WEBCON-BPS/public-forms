using Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WEBCON.FormsGenerator.Presentation.ViewModels
{
    public class ConfigurationViewModel
    {
        [DisplayName("Portal Url")]
        [StringLength(100, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(SharedResource))]
        public string ApiUrl {get;set;}
        [DisplayName("Portal ClientId")]
        [StringLength(100, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(SharedResource))]
        public string ClientId { get; set; }
        [DisplayName("Portal ClientSecret")]
        [StringLength(100, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(SharedResource))]
        public string ClientSecret { get; set; }
        [DisplayName("Portal Database ID")]
        [Range(1, short.MaxValue, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "Range")]     
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]  
        public int DatabaseId { get; set; }
        [DisplayName("Captcha API key")]
        [StringLength(100, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(SharedResource))]
        public string CaptchaApiKey { get; set; }
        public string LoggingLevel { get; set; }
        public bool IsHostedOnAzure { get; set; }
    }
}
