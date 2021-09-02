using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Interface
{
    /// <summary>
    /// Service to get BPS API client objects
    /// </summary>
    public interface IBpsQueryService
    {
        Task<IEnumerable<DTO.BpsApplication>> GetApplicationsAsync();
        Task<IEnumerable<DTO.BpsProcess>> GetProcessesAsync(Guid applicatioGuid);
        Task<IEnumerable<DTO.BpsWorkflow>> GetWorkflowsAsync(Guid processGuid);
        Task<DTO.BpsStep> GetStartStepAsync(Guid workflowGuid);
        Task<IEnumerable<DTO.BpsPath>> GetStepPathsAsync(Guid stepGuid);
        Task<IEnumerable<DTO.BpsFormType>> GetAssociatedFormTypesAsync(Guid workflowGuid);
        Task<IEnumerable<DTO.FormField>> GetFormFieldsAsync(Guid formTypeGuid, Guid workflowGuid, Guid stepGuid, string token = "");
        Task<IEnumerable<DTO.BpsBusinessEntity>> GetBusinessEntities();
        Task<string> Authenticate();
    }
}
