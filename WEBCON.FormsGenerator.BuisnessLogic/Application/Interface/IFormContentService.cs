using System.Collections.Generic;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Interface
{
    /// <summary>
    /// Definition for creating form content (generated form body)
    /// </summary>
    public interface IFormContentService
    {
        /// <summary>
        /// Creates form content based on provided fields
        /// </summary>
        /// <param name="formFields">fields to add on form</param>
        /// <param name="contentTitle">form title</param>
        /// <returns></returns>
        FormContent CreateFormContent(IEnumerable<FormField> formFields, string contentTitle);
        /// <summary>
        /// Change async metadata labels on form with selected values
        /// </summary>
        /// <param name="formContentFields">List of content fields</param>
        /// <param name="formWithMetadata">Form content to transform</param>
        /// <returns></returns>
        Task<string> GetFormContentWithTransformedMetadataAsync(IEnumerable<FormContentField> formContentFields, string formWithMetadata, string contentTitle, string contentDescription, string contentSubmitText);
        /// <summary>
        /// Change metadata labels on form with selected values
        /// </summary>
        /// <param name="formContentFields">List of content fields</param>
        /// <param name="formWithMetadata">Form content to transform</param>
        /// <returns></returns>
        string GetFormContentWithTransformedMetadata(IEnumerable<FormContentField> contentFields, string formWithMetadata, string contentTitle, string contentDescription, string contentSubmitText);
        /// <summary>
        /// Create form content input based on incoming field definition
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        string CreateFormContentInput(DTO.FormField field);
    }
}
