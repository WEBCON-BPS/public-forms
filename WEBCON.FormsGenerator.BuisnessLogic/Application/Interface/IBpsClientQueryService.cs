using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Interface
{
    /// <summary>
    /// Definition for BPS API provider
    /// </summary>
    public interface IBpsClientQueryService
    {
        /// <summary>
        /// Get available applications for authenticated user
        /// </summary>
        /// <returns>List of applications</returns>
        Task<IEnumerable<BpsApplication>> GetApplicationsAsync();
        /// <summary>
        /// Get available processes for selected application
        /// </summary>
        /// <param name="applicationGuid">Selected application Guid</param>
        /// <returns>List of processes</returns>
        Task<IEnumerable<BpsProcess>> GetProcessesAsync(Guid applicationGuid);
        /// <summary>
        /// Get available workflows for selected process
        /// </summary>
        /// <param name="processGuid">Selected process Guid</param>
        /// <returns>List of workflows</returns>
        Task<IEnumerable<BpsWorkflow>> GetWorkflowsAsync(Guid processGuid);
        /// <summary>
        /// Get start step definition for selected workflow
        /// </summary>
        /// <param name="workflowGuid">Selected workflow Guid</param>
        /// <returns>Definition of start step</returns>
        Task<BpsStep> GetStartStepAsync(Guid workflowGuid);
        /// <summary>
        /// Get step paths list
        /// </summary>
        /// <param name="stepGuid"></param>
        /// <returns></returns>
        Task<IEnumerable<BpsPath>> GetStepPaths(Guid stepGuid);
        /// <summary>
        /// Get forms types list connected with selected workflow
        /// </summary>
        /// <param name="workflowGuid">Selected workflow Guid</param>
        /// <returns>List of form types</returns>
        Task<IEnumerable<BpsFormType>> GetAssociatedFormTypesAsync(Guid workflowGuid);
        /// <summary>
        /// Get form fields list for selected form type without values
        /// </summary>
        /// <param name="formTypeGuid">Selected form type Guid</param>
        /// <param name="workflowGuid">Workflow Guid</param>
        /// <param name="stepGuid">Start step Guid</param>
        /// <returns>List of form fields</returns>
        Task<IEnumerable<FormField>> GetFormFieldsAsync(Guid formTypeGuid, Guid workflowGuid, Guid stepGuid, string token = "");
        /// <summary>
        /// Get form fields list for selected form type with values
        /// </summary>
        IAsyncEnumerable<FormField> GetFormFieldsWithValuesAsync(Guid formTypeGuid, Guid workflowGuid, Guid stepGuid, string token = "");
        /// <summary>
        /// Get business entities (companies) from BPS
        /// </summary>
        /// <returns>List of business entities</returns>
        Task<IEnumerable<BpsBusinessEntity>> GetBusinessEntities();
        Task<string> Authenticate();
    }
}
