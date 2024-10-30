using System;
using System.Collections.Generic;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Interface
{
    /// <summary>
    /// Definition for implementation form content parts builder
    /// </summary>
    public interface IFormBuilder
    {
        /// <summary>
        /// Put created fields into form
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="formAction">Action which has to be executed after form submit</param>
        string EmbedFieldsInForm(string fields);
        /// <summary>
        /// Create form input
        /// </summary>
        string CreateFormFieldBasedOnBpsField(FormField bpsFormField);      
        /// <summary>
        /// Creates label for field input
        /// </summary>
        /// <param name="labelId">Label identifier</param>
        /// <param name="labelName">Label name</param>
        /// <returns>Label body</returns>
        string CreateLabel(string labelId, string labelName);
        /// <summary>
        /// Creates space for form name.
        /// </summary>
        string CreateFormNameArea(string formName);
        /// <summary>
        /// Creates space for description
        /// </summary>
        string CreateFormDescriptionArea(string formDescription);
        /// <summary>
        /// Replace metadata label on form with selected metadata proper value
        /// </summary>
        string TransformMetadataOnForm(IEnumerable<FormContentField> formContentFields, string formContent, string contentTitle, string contentDescription = "", string contentSubmitText = "Submit");
        /// <summary>
        /// Embed selected inputs array in specified group in form
        /// </summary>
        /// <param name="inputs">form inputs to group</param>
        /// <returns>Inputs in group</returns>
        string EmbedInputsInGroup(IEnumerable<string> inputs, string groupId);
        /// <summary>
        /// Add new fields to existing form
        /// </summary>
        /// <param name="fields">New fields</param>
        /// <param name="existingFormContent">Existing form content</param>
        /// <returns></returns>
        string EmbedInputsInExistingForm(IEnumerable<string> fields, string existingFormContent);
        /// <summary>
        /// Update choices for inputs with options 
        /// </summary>
        /// <param name="formField">Form field definition</param>
        /// <param name="formContent">Current form content body</param>
        /// <returns></returns>
        string UpdateChoices(DTO.FormField formField, string formContent);

        /// <summary>
        /// Remove fields from existing form content
        /// </summary>
        /// <param name="bpsformFieldGuid">BPS form field guid</param>
        /// <param name="formContent">Existing form content</param>
        /// <returns></returns>
        string RemoveFieldFromExistingForm(Guid bpsformFieldGuid, string formContent);
        string UpdateField(Guid formContentFieldGuid, string formContent, string updatedHtml);
    }
}
