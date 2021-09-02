using System;
using System.Collections.Generic;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.DTO
{
    public class Form : BaseObject
    {
        public int Id { get; set; }
        public Guid BpsApplicationGuid { get; set; }
        public string BpsApplicationName { get; set; }
        public Guid BpsProcessGuid { get; set; }
        public string BpsProcessName { get; set; }
        public Guid BpsWorkflowGuid { get; set; }
        public string BpsWorkflowName { get; set; }
        public Guid BpsFormTypeGuid { get; set; }
        public string BpsFormTypeName { get; set; }
        public Guid BpsWorkflowStartStepGuid { get; set; }
        public string BpsWorkflowStartStepName { get; set; }
        public Guid BpsStartStepPathGuid { get; set; }
        public string BpsStartStepPathName { get; set; }
        public Guid BpsBusinessEntityGuid { get; set; }
        public string BpsBusinessEntityName { get; set; }
        public string Content { get; set; }
        public string ContentTransformed { get; set; }
        public List<FormContentField> ContentFields { get; set; }
        public bool IsCaptchaRequired { get; set; }
        public string Style { get; set; }
        public string[] FrameAncestors { get; set; }
        public string CustomClientId { get; set; }
        public string CustomClientSecret { get; set; }
        public bool UseStandardBootstrapStyle { get; set; }
        public string CustomCssLink { get; set; }
        public string ContentTitle { get; set; }
        public string ContentDescription { get; set; }
        public string ContentSubmitText { get; set; }
        public string ContentSubmitProcessingText { get; set; }
        public string ContentSubmitSuccessMessage { get; set; }
        public bool IsActive { get; set; }
    }
}
