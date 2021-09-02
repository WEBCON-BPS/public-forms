using System;
using System.Collections.Generic;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model
{
    public class BpsWorkflow : BpsEntity
    {
        public BpsWorkflow()
        {
        }
        public BpsWorkflow(Guid guid, string name, BpsProcess process) : base(guid, name)
        {
            BpsProcess = process ?? throw new BpsEntityArgumentException("Workflow process is not specified");
        }
        public BpsProcess BpsProcess { get; protected set; }
        public ICollection<BpsWorkflowStep> BpsWorkflowSteps { get; set; }
        public void Update(BpsProcess process)
        {
            BpsProcess = process ?? throw new BpsEntityArgumentException("Workflow process is not specified");
        }
    }
}
