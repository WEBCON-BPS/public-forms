using System;
using System.ComponentModel.DataAnnotations;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model
{
    public class Form : Entity
    {
        public Form()
        {

        }
        public Form(string name, string content, BpsFormType formType, BpsStepPath stepPath)
        {
            Validate(name, formType, stepPath);
            Guid = Guid.NewGuid();
            Created = DateTime.Now;
            Name = name;
            Content = content;
            BpsStepPath = stepPath;
            BpsFormType = formType;
        }
        public DateTime Created { get; private set; }
        public DateTime Modified { get; private set; }
        [StringLength(50)]
        public string Name { get; private set; }
        public string Content { get; private set; }
        public BpsStepPath BpsStepPath { get; private set; }
        public BpsFormType BpsFormType { get; private set; }
        public string Style { get; private set; }
        public bool? IsCaptchaRequired { get; private set; }
        public string FrameAncestors { get; private set; }
        [StringLength(50)]
        public string ClientId { get; private set; }
        [StringLength(500)]
        public string ClientSecret { get; private set; }
        public bool UseStandardBootstrapStyle { get; private set; }
        public string CustomCssLink { get; private set; }
        public string ContentTitle { get; private set; }
        public string ContentDescription { get; private set; }
        public string ContentSubmitText { get; private set; }
        public string ContentSubmitProcessingText { get; private set; }
        public string ContentSubmitSuccessMessage { get; private set; }
        public bool IsActive { get; private set; }   
        public Guid BusinessEntityGuid { get; set; }
        public void Update(string name, string content, BpsFormType formType, BpsStepPath stepPath)
        {
            Validate(name, formType, stepPath);
            Name = name;
            SetContent(content);
            BpsFormType = formType;
            BpsStepPath = stepPath;
            Modified = DateTime.Now;
        }
        public void SetBusinessEntity(Guid businessEntityGuid)
        {
            this.BusinessEntityGuid = businessEntityGuid;
        }
        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }
        public void SetCustomStyleLink(string customCssLink)
        {
            CustomCssLink = customCssLink;
        }
        public void SetCustomStyle(string style)
        {
            Style = style;
        }
        public void SetFrameAncestors(string frameAncestors)
        {
            FrameAncestors = frameAncestors;
        }
        public void SetCaptchaRequired(bool isRequired)
        {
            IsCaptchaRequired = isRequired;
        }
        public void SetUseStandardBootstrapStyle(bool useStandardStyle)
        {
            UseStandardBootstrapStyle = useStandardStyle;
        }
        public void SetCustomCredentials(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
        public void SetContentTexts(string contentTitle, string contentDescription, string contentSubmitText, string contentSubmitProcessingMessage, string contentSubmitSuccessMessage)
        {
            ContentTitle = contentTitle;
            ContentDescription = contentDescription;
            ContentSubmitText = contentSubmitText;
            ContentSubmitProcessingText = contentSubmitProcessingMessage;
            ContentSubmitSuccessMessage = contentSubmitSuccessMessage;
        }
        public void SetContent(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ApplicationArgumentException("Content cannot be empty");
            Content = content;
        }
        private void Validate(string name, BpsFormType formtype, BpsStepPath stepPath)
        {
            if (formtype == null)
                throw new ApplicationArgumentException("Form type has to be selected");
            if (stepPath == null)
                throw new ApplicationArgumentException("Step path has to be selected");
            if (string.IsNullOrEmpty(name))
                throw new ApplicationArgumentException("Name cannot be empty");
        }
    }
}
