using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WEBCON.FormsGenerator.Presentation.ViewModels
{
    public class FormViewModel
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DisplayName("BPS Application Guid")]
        [RegularExpression("^(?!--Select--).*$", ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public Guid BpsApplicationGuid { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DisplayName("BPS Application")]
        public string BpsApplicationName { get; set; }
        [RegularExpression("^(?!--Select--).*$", ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DisplayName("BPS Process Guid")]
        public Guid BpsProcessGuid { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DisplayName("BPS Process")]
        public string BpsProcessName { get; set; }

        [RegularExpression("^(?!--Select--).*$", ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DisplayName("BPS Workflow Guid")]
        public Guid BpsWorkflowGuid { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DisplayName("BPS Workflow")]
        public string BpsWorkflowName { get; set; }

        [RegularExpression("^(?!--Select--).*$", ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DisplayName("BPS Form Type Guid")]
        public Guid BpsFormTypeGuid { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DisplayName("BPS Form Type")]
        public string BpsFormTypeName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [RegularExpression(@"(.|\s)*\S(.|\s)*", ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DisplayName("BPS Start Step Guid")]
        public Guid BpsWorkflowStartStepGuid { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DisplayName("BPS Start Step")]
        public string BpsWorkflowStartStepName { get; set; }

        [DisplayName("BPS Start Step Path Guid")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [RegularExpression("^(?!--Select--).*$", ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public Guid BpsStartStepPathGuid { get; set; }

        [DisplayName("BPS Start Step Path")]
        public string BpsStartStepPathName { get; set; }

        [DisplayName("BPS Business Entity Guid")]
        [RegularExpression("^(?!--Select--).*$", ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public Guid BpsBusinessEntityGuid { get; set; }
        
        [DisplayName("BPS Business Entity")]
        public string BpsBusinessEntityName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(50, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(SharedResource))]
        [DisplayName("Name")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DisplayName("Form body")]
        public string Content { get; set; }
        public string ContentTransformed { get; set; }
        public List<FormContentFieldViewModel> ContentFields { get; set; }
        [StringLength(5000, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Style { get; set; }
        [DisplayName("Require reCaptcha")]
        public bool IsCaptchaRequired { get; set; }
        [DisplayName("Generated IFrame")]
        public string IFrame { get; set; }
        public string FrameUrl { get; set; }
        [DisplayName("Frame ancestors")]
        public string[] FrameAncestors { get; set; }
        public bool CustomConnectionSelected { get; set; }
        [DisplayName("Client ID")]
        public string CustomClientId { get; set; }
        [DisplayName("Client Secret")]
        public string CustomClientSecret { get; set; }
        [DisplayName("Use standard Bootstrap style")]
        public bool UseStandardBootstrapStyle { get; set; }
        [DisplayName("Use custom CSS stylesheet")]
        public string CustomCssLink { get; set; }
        [DisplayName("Form title")]
        public string ContentTitle { get; set; }
        [DisplayName("Form description")]
        public string ContentDescription { get; set; }
        [DisplayName("Form submit text")]
        public string ContentSubmitText { get; set; }
        [DisplayName("Form content submit processing text")]
        public string ContentSubmitProcessingText { get; set; }
        [DisplayName("Form submit success message")]
        public string ContentSubmitSuccessMessage { get; set; }
        public Dictionary<string,string> MessageParameters { get; set; }
        [DisplayName("Active")]
        public bool IsActive { get; set; }
    }
}
