using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Interface
{
    /// <summary>
    /// Definition for refreshing form content (generated form body) 
    /// </summary>
    public interface IFormContentRefreshService
    {
        /// <summary>
        /// Refresh form content - add new end remove deleted fields from content
        /// </summary>
        /// <param name="formFields">All current form fields</param>
        /// <param name="formGuid">Refreshed form guid</param>
        /// <returns></returns>
        Task<FormContent> RefreshFormContentAsync(IEnumerable<FormField> formFields, Guid formGuid);
    }
}
