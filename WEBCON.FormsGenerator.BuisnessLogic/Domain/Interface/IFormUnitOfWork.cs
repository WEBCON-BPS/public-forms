using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Interface
{
    public interface IFormUnitOfWork : IDisposable
    {
        IFormRepository Forms { get; set; }
        IRepository<BpsApplication> BpsApplications { get; set; }
        IRepository<BpsFormType> BpsFormTypes { get; set; }
        IRepository<BpsFormField> BpsFormFields { get; set; }
        IRepository<BpsProcess> BpsProcesses { get; set; }
        IRepository<BpsWorkflow> BpsWorkflows { get; set; }
        IRepository<BpsWorkflowStep> BpsWorkflowSteps { get; set; }
        IRepository<BpsStepPath> BpsStepPaths { get; set; }
        IRepository<FormContentField> FormContentFields { get; set; }
        IRepository<BpsBusinessEntity> BpsBusinessEntity { get; set; }
        int SaveChanges();
    }
}
