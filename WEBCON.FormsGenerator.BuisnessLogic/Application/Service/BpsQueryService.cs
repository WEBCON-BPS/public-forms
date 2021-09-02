using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Service
{
    public class BpsQueryService : IBpsQueryService
    {
        private readonly IBpsClientQueryService clientService;
        public BpsQueryService(IBpsClientQueryService clientService)
        {
            this.clientService = clientService ?? throw new ApplicationArgumentException("Client service instance has not been provided");
        }
        public Task<string> Authenticate()
        {
            return clientService.Authenticate();
        }
        public Task<IEnumerable<DTO.BpsApplication>> GetApplicationsAsync()
        {
            return clientService.GetApplicationsAsync();
        }
        public Task<IEnumerable<BpsFormType>> GetAssociatedFormTypesAsync(Guid workflowGuid)
        {
            if (workflowGuid == Guid.Empty)
                throw new ApplicationArgumentException("Workflow guid has not been provided. Could not get form types.");
            return clientService.GetAssociatedFormTypesAsync(workflowGuid);
        }
        public Task<IEnumerable<BpsBusinessEntity>> GetBusinessEntities()
        {
            return clientService.GetBusinessEntities();
        }
        public async Task<IEnumerable<FormField>> GetFormFieldsAsync(Guid formTypeGuid, Guid workflowGuid, Guid stepGuid, string token = "")
        {
            if (formTypeGuid == Guid.Empty)
                throw new ApplicationArgumentException("Form type guid has not been provided. Could not get form body.");
            if (workflowGuid == Guid.Empty)
                throw new ApplicationArgumentException("Workflow guid has not been provided. Could not get form body.");
            if (stepGuid == Guid.Empty)
                throw new ApplicationArgumentException("Step guid has not been provided. Could not get form body.");

            return await clientService.GetFormFieldsWithValuesAsync(formTypeGuid, workflowGuid, stepGuid, token).ToListAsync();                     
        }

        public Task<IEnumerable<BpsProcess>> GetProcessesAsync(Guid applicationGuid)
        {      
            if (applicationGuid == Guid.Empty)
                throw new ApplicationArgumentException("Application guid has not been provided. Could not get processes.");

            return clientService.GetProcessesAsync(applicationGuid);
        }
        public Task<BpsStep> GetStartStepAsync(Guid workflowGuid)
        {
            if (workflowGuid == Guid.Empty)
                throw new ApplicationArgumentException("Workflow guid has not been provided. Could not get start step.");

            return clientService.GetStartStepAsync(workflowGuid);
        }

        public Task<IEnumerable<BpsPath>> GetStepPathsAsync(Guid stepGuid)
        {
            if (stepGuid == Guid.Empty)
                throw new ApplicationArgumentException("Step guid has not been provided. Could not get step paths.");

            return clientService.GetStepPaths(stepGuid);
        }

        public Task<IEnumerable<BpsWorkflow>> GetWorkflowsAsync(Guid processGuid)
        {         
            if (processGuid == Guid.Empty)
                throw new ApplicationArgumentException("Process guid has not been provided. Could not get workflows.");
            return clientService.GetWorkflowsAsync(processGuid);
        }  
    }
}
