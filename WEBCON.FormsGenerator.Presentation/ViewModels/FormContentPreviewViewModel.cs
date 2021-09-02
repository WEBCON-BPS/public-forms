using System;

namespace WEBCON.FormsGenerator.Presentation.ViewModels
{
    public class FormContentPreviewViewModel
    {
        public Guid FormGuid { get; set; }
        public string Html { get; set; }
        public bool IsCaptchaRequired { get; set; }
        public string CaptchaKey { get; set; }
        public string CustomCss { get; set; }
        public bool UseStandardBootstrapStyle { get; set; }
        public string CustomCssLink { get; set; }
        public bool IsReadonly { get; set; }
        public string ContentSubmitProcessingText { get; set; }
        public string ContentSubmitSuccessMessage { get; set; }
        public bool IsActive { get; set; }
    }
}
