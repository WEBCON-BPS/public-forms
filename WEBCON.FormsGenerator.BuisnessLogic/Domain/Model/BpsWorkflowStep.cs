using System;
using System.Collections.Generic;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model
{
    public class BpsWorkflowStep : BpsEntity
    {
        public BpsWorkflowStep()
        {

        }
        public BpsWorkflowStep(Guid guid, string name, bool isStartStep, BpsWorkflow workflow) : base(guid, name)
        {
            BpsWorkflow = workflow ?? throw new BpsEntityArgumentException("Step workflow is not specified");
            IsStartStep = isStartStep;
        }
        public BpsWorkflow BpsWorkflow { get; set; }
        public bool IsStartStep { get; protected set; }
        public ICollection<BpsStepPath> BpsStepPaths { get; set; }
        public void Update(BpsWorkflow workflow)
        {
            BpsWorkflow = workflow ?? throw new BpsEntityArgumentException("Step workflow is not specified");
        }
    }
}
